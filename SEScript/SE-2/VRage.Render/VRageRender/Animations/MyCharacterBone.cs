using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Bones in this model are represented by this class, which
	/// allows a bone to have more detail associatd with it.
	///
	/// This class allows you to manipulate the local coordinate system
	/// for objects by changing the scaling, translation, and rotation.
	/// These are indepenent of the bind transformation originally supplied
	/// for the model. So, the actual transformation for a bone is
	/// the product of the:
	///
	/// Scaling
	/// Bind scaling (scaling removed from the bind transform)
	/// Rotation
	/// Translation
	/// Bind Transformation
	/// Parent Absolute Transformation
	///
	/// </summary>
	public class MyCharacterBone
	{
		/// <summary>
		/// Any parent for this bone
		/// </summary>
		private readonly MyCharacterBone m_parent;

		private readonly List<MyCharacterBone> m_children;

		/// <summary>
		/// The bind transform is the transform for this bone
		/// as loaded from the original model. It's the base pose.
		/// I do remove any scaling, though.
		/// </summary>
		private Matrix m_bindTransform = Matrix.Identity;

		private Matrix m_bindTransformInv = Matrix.Identity;

		private Quaternion m_bindRotationInv = Quaternion.Identity;

		/// <summary>
		/// Any translation applied to the bone
		/// </summary>
		private Vector3 m_translation = Vector3.Zero;

		/// <summary>
		/// Any rotation applied to the bone
		/// </summary>
		private Quaternion m_rotation = Quaternion.Identity;

		/// <summary>
		/// indicates whether bone needs recalculation
		/// </summary>
		private bool m_changed = true;

		/// <summary>
		/// The bone name
		/// </summary>
		public string Name = "";

		private Matrix[] m_relativeStorage;

		private Matrix[] m_absoluteStorage;

		public int Index
		{
			get;
			private set;
		}

		/// <summary>
		/// The bone bind transform
		/// </summary>
		public Matrix BindTransform => m_bindTransform;

		public Matrix BindTransformInv => m_bindTransformInv;

		/// <summary>
		/// Inverse of absolute bind transform for skinnning
		/// </summary>
		public Matrix SkinTransform
		{
			get;
			set;
		}

		/// <summary>
		/// Bone rotation
		/// </summary>
		public Quaternion Rotation
		{
			get
			{
				return m_rotation;
			}
			set
			{
				m_rotation = value;
				m_changed = true;
			}
		}

		/// <summary>
		/// Any translations
		/// </summary>
		public Vector3 Translation
		{
			get
			{
				return m_translation;
			}
			set
			{
				m_translation = value;
				m_changed = true;
			}
		}

		/// <summary>
		/// The parent bone or null for the root bone
		/// </summary>
		public MyCharacterBone Parent => m_parent;

		/// <summary>
		/// The bone absolute transform
		/// </summary>
		public Matrix AbsoluteTransform => m_absoluteStorage[Index];

		/// <summary>
		/// The bone absolute transform
		/// </summary>
		public Matrix RelativeTransform => m_relativeStorage[Index];

		/// <summary>
		/// Has this bone or any parent bone changed?
		/// </summary>
		private bool HasThisOrAnyParentChanged
		{
			get
			{
				MyCharacterBone myCharacterBone = this;
				do
				{
					if (myCharacterBone.m_changed)
					{
						return true;
					}
					myCharacterBone = myCharacterBone.Parent;
				}
				while (myCharacterBone != null);
				return false;
			}
		}

		public int Depth
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor for a bone object
		/// </summary>
		/// <param name="name">The name of the bone</param>
		/// <param name="bindTransform">The initial bind transform for the bone</param>
		/// <param name="parent">A parent for this bone</param>
		/// <param name="index">Index of this bone in storage arrays.</param>
		/// <param name="relativeStorage">reference to matrix array storing all relative transforms of the skeleton</param>
		/// <param name="absoluteStorage">reference to matrix array storing all absolute transforms of the skeleton</param>
		public MyCharacterBone(string name, MyCharacterBone parent, Matrix bindTransform, int index, Matrix[] relativeStorage, Matrix[] absoluteStorage)
		{
			Index = index;
			m_relativeStorage = relativeStorage;
			m_absoluteStorage = absoluteStorage;
			Name = name;
			m_parent = parent;
			Depth = GetHierarchyDepth();
			m_bindTransform = bindTransform;
			m_bindTransformInv = Matrix.Invert(bindTransform);
			m_bindRotationInv = Quaternion.CreateFromRotationMatrix(m_bindTransformInv);
			m_children = new List<MyCharacterBone>();
			if (m_parent != null)
			{
				m_parent.AddChild(this);
			}
			ComputeAbsoluteTransform();
			SkinTransform = Matrix.Invert(AbsoluteTransform);
		}

		static MyCharacterBone()
		{
		}

		private int GetHierarchyDepth()
		{
			int num = 0;
			MyCharacterBone parent = m_parent;
			while (parent != null)
			{
				parent = parent.Parent;
				num++;
			}
			return num;
		}

		/// <summary>
		/// Compute absolute bone transforms for whole hierarchy.
		/// Expects the array to be sorted by depth in hiearachy.
		/// </summary>
		public static void ComputeAbsoluteTransforms(MyCharacterBone[] bones)
		{
			MyCharacterBone[] array = bones;
			foreach (MyCharacterBone myCharacterBone in array)
			{
				if (myCharacterBone.Parent != null)
				{
					myCharacterBone.m_changed = (myCharacterBone.ComputeBoneTransform() || myCharacterBone.Parent.m_changed);
					if (myCharacterBone.m_changed)
					{
						Matrix.Multiply(ref myCharacterBone.m_relativeStorage[myCharacterBone.Index], ref myCharacterBone.m_absoluteStorage[myCharacterBone.Parent.Index], out myCharacterBone.m_absoluteStorage[myCharacterBone.Index]);
					}
				}
				else
				{
					myCharacterBone.m_changed = myCharacterBone.ComputeBoneTransform();
					if (myCharacterBone.m_changed)
					{
						myCharacterBone.m_absoluteStorage[myCharacterBone.Index] = myCharacterBone.m_relativeStorage[myCharacterBone.Index];
					}
				}
			}
			array = bones;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].m_changed = false;
			}
		}

		/// <summary>
		/// Translate all bones. Translation vector is given in model space. 
		/// We expect that absolute transforms are already computed.
		/// </summary>
		public static void TranslateAllBones(MyCharacterBone[] characterBones, Vector3 translationModelSpace)
		{
			if (characterBones == null || characterBones.Length < 0)
			{
				return;
			}
			foreach (MyCharacterBone myCharacterBone in characterBones)
			{
				if (myCharacterBone.Parent == null)
				{
					myCharacterBone.Translation += translationModelSpace;
					myCharacterBone.ComputeBoneTransform();
					myCharacterBone.m_changed = false;
				}
				myCharacterBone.m_absoluteStorage[myCharacterBone.Index].Translation += translationModelSpace;
			}
		}

		/// <summary>
		/// Compute the absolute transformation for this bone.
		/// </summary>
		public void ComputeAbsoluteTransform(bool propagateTransformToChildren = true)
		{
			if (HasThisOrAnyParentChanged)
			{
				m_changed = ComputeBoneTransform();
				if (Parent != null)
				{
					Matrix.Multiply(ref m_relativeStorage[Index], ref m_absoluteStorage[Parent.Index], out m_absoluteStorage[Index]);
				}
				else
				{
					m_absoluteStorage[Index] = m_relativeStorage[Index];
				}
				if (propagateTransformToChildren)
				{
					PropagateTransform();
				}
				m_changed = false;
			}
		}

		public bool ComputeBoneTransform()
		{
			if (m_changed)
			{
				Matrix.CreateFromQuaternion(ref m_rotation, out m_relativeStorage[Index]);
				m_relativeStorage[Index].M41 = m_translation.X;
				m_relativeStorage[Index].M42 = m_translation.Y;
				m_relativeStorage[Index].M43 = m_translation.Z;
				Matrix.Multiply(ref m_relativeStorage[Index], ref m_bindTransform, out m_relativeStorage[Index]);
				m_changed = false;
				return true;
			}
			return false;
		}

		/// <summary>
		/// This sets the rotation and translation such that the
		/// rotation times the translation times the bind after set
		/// equals this matrix. This is used to set animation values.
		/// </summary>
		public void SetCompleteTransform(ref Vector3 translation, ref Quaternion rotation, float weight)
		{
			m_changed = true;
			Vector3.Transform(ref translation, ref m_bindTransformInv, out Vector3 result);
			Translation = Vector3.Lerp(Translation, result, weight);
			Quaternion.Multiply(ref m_bindRotationInv, ref rotation, out Quaternion result2);
			Rotation = Quaternion.Slerp(Rotation, result2, weight);
		}

		/// <summary>
		/// This sets the rotation and translation of the rest pose.
		/// </summary>
		public void SetCompleteBindTransform()
		{
			m_changed = true;
			Translation = Vector3.Zero;
			Rotation = Quaternion.Identity;
		}

		/// <summary>
		/// This sets the rotation and translation such that the
		/// rotation times the translation times the bind after set
		/// equals this matrix. This is used to set animation values.
		/// </summary>
		public void SetCompleteTransform(ref Vector3 translation, ref Quaternion rotation)
		{
			Vector3.Transform(ref translation, ref m_bindTransformInv, out m_translation);
			Quaternion.Multiply(ref m_bindRotationInv, ref rotation, out m_rotation);
			m_changed = true;
		}

		/// <summary>
		/// Set the rotation and translation of the bone from absolute transform. Does not recompute hierarchy - call ComputeAbsoluteTransform.
		/// </summary>
		/// <param name="absoluteMatrix">absolute transform</param>
		/// <param name="onlyRotation">apply only rotation</param>
		public void SetCompleteTransformFromAbsoluteMatrix(ref Matrix absoluteMatrix, bool onlyRotation)
		{
			Matrix matrix = Matrix.Identity;
			if (Parent != null)
			{
				matrix = Parent.AbsoluteTransform;
			}
			Matrix matrix2 = absoluteMatrix * Matrix.Invert(matrix) * m_bindTransformInv;
			Rotation = Quaternion.CreateFromRotationMatrix(matrix2);
			if (!onlyRotation)
			{
				Translation = matrix2.Translation;
			}
		}

		/// <summary>
		/// Set the rotation and translation of the bone from absolute transform. Does not recompute hierarchy - call ComputeAbsoluteTransform.
		/// </summary>
		/// <param name="absoluteMatrix">absolute transform</param>
		/// <param name="onlyRotation">apply only rotation</param>
		public void SetCompleteTransformFromAbsoluteMatrix(Matrix absoluteMatrix, bool onlyRotation)
		{
			SetCompleteTransformFromAbsoluteMatrix(ref absoluteMatrix, onlyRotation);
		}

		/// <summary>
		/// This adds the rotation and translation to the one that is already set inside.
		/// </summary>
		public void SetCompleteRotation(ref Quaternion rotation)
		{
			Quaternion.Multiply(ref m_bindRotationInv, ref rotation, out m_rotation);
			m_changed = true;
		}

		/// <summary>
		/// Same as SetCompleteTransform, but result is not stored internally, it is returned instead.
		/// </summary>
		public void GetCompleteTransform(ref Vector3 translation, ref Quaternion rotation, out Vector3 completeTranslation, out Quaternion completeRotation)
		{
			Vector3.Transform(ref translation, ref m_bindTransformInv, out completeTranslation);
			Quaternion.Multiply(ref m_bindRotationInv, ref rotation, out completeRotation);
		}

		internal void SetBindTransform(Matrix bindTransform)
		{
			m_changed = true;
			m_bindTransform = bindTransform;
			m_bindTransformInv = Matrix.Invert(bindTransform);
			m_bindRotationInv = Quaternion.CreateFromRotationMatrix(m_bindTransformInv);
		}

		internal void AddChild(MyCharacterBone child)
		{
			m_children.Add(child);
		}

		private void PropagateTransform()
		{
			foreach (MyCharacterBone child in m_children)
			{
				child.ComputeAbsoluteTransform();
			}
		}

		/// <summary>
		/// Returns bone's rig absolute transform - including transforms of all parent bones
		/// </summary>
		/// <returns></returns>
		public Matrix GetAbsoluteRigTransform()
		{
			MyCharacterBone parent = m_parent;
			if (parent != null)
			{
				Matrix bindTransform = m_bindTransform;
				while (parent != null)
				{
					bindTransform *= parent.m_bindTransform;
					parent = parent.Parent;
				}
				return bindTransform;
			}
			return m_bindTransform;
		}

		public MyCharacterBone GetChildBone(int childIndex)
		{
			if (m_children == null || childIndex < 0 || childIndex >= m_children.Count)
			{
				return null;
			}
			return m_children[childIndex];
		}

		public override string ToString()
		{
			return Name + " [MyCharacterBone]";
		}
	}
}
