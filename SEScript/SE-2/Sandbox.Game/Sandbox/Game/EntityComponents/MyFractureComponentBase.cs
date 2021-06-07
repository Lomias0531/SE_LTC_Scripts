using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.EntityComponents
{
	public abstract class MyFractureComponentBase : MyEntityComponentBase
	{
		public struct Info
		{
			public MyEntity Entity;

			public HkdBreakableShape Shape;

			public bool Compound;
		}

		protected readonly List<HkdShapeInstanceInfo> m_tmpChildren = new List<HkdShapeInstanceInfo>();

		protected readonly List<HkdShapeInstanceInfo> m_tmpShapeInfos = new List<HkdShapeInstanceInfo>();

		protected readonly List<MyObjectBuilder_FractureComponentBase.FracturedShape> m_tmpShapeList = new List<MyObjectBuilder_FractureComponentBase.FracturedShape>();

		public HkdBreakableShape Shape;

		public abstract MyPhysicalModelDefinition PhysicalModelDefinition
		{
			get;
		}

		public override string ComponentTypeDebugString => "Fracture";

		public override bool IsSerialized()
		{
			return true;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			MyRenderComponentFracturedPiece myRenderComponentFracturedPiece = new MyRenderComponentFracturedPiece();
			if (base.Entity.Render.ModelStorage != null)
			{
				myRenderComponentFracturedPiece.ModelStorage = base.Entity.Render.ModelStorage;
			}
			base.Entity.Render.UpdateRenderObject(visible: false);
			MyPersistentEntityFlags2 persistentFlags = base.Entity.Render.PersistentFlags;
			Vector3 colorMaskHsv = base.Entity.Render.ColorMaskHsv;
			Dictionary<string, MyTextureChange> textureChanges = base.Entity.Render.TextureChanges;
			bool metalnessColorable = base.Entity.Render.MetalnessColorable;
			base.Entity.Render = myRenderComponentFracturedPiece;
			base.Entity.Render.NeedsDraw = true;
			base.Entity.Render.PersistentFlags |= (persistentFlags | MyPersistentEntityFlags2.CastShadows);
			base.Entity.Render.ColorMaskHsv = colorMaskHsv;
			base.Entity.Render.TextureChanges = textureChanges;
			base.Entity.Render.MetalnessColorable = metalnessColorable;
			base.Entity.Render.EnableColorMaskHsv = false;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (Shape.IsValid())
			{
				Shape.RemoveReference();
			}
		}

		public virtual bool RemoveChildShapes(IEnumerable<string> shapeNames)
		{
			m_tmpShapeList.Clear();
			GetCurrentFracturedShapeList(m_tmpShapeList, shapeNames);
			RecreateShape(m_tmpShapeList);
			m_tmpShapeList.Clear();
			return false;
		}

		protected void GetCurrentFracturedShapeList(List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList, IEnumerable<string> excludeShapeNames = null)
		{
			GetCurrentFracturedShapeList(Shape, shapeList, excludeShapeNames);
		}

		private static bool GetCurrentFracturedShapeList(HkdBreakableShape breakableShape, List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList, IEnumerable<string> excludeShapeNames = null)
		{
			if (!breakableShape.IsValid())
			{
				return false;
			}
			string name = breakableShape.Name;
			bool flag = string.IsNullOrEmpty(name);
			if (excludeShapeNames != null && !flag)
			{
				foreach (string excludeShapeName in excludeShapeNames)
				{
					if (name == excludeShapeName)
					{
						return false;
					}
				}
			}
			MyObjectBuilder_FractureComponentBase.FracturedShape item;
			if (breakableShape.GetChildrenCount() > 0)
			{
				List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
				breakableShape.GetChildren(list);
				bool flag2 = true;
				foreach (HkdShapeInstanceInfo item2 in list)
				{
					flag2 &= GetCurrentFracturedShapeList(item2.Shape, shapeList, excludeShapeNames);
				}
				if (!flag && flag2)
				{
					foreach (HkdShapeInstanceInfo inst in list)
					{
						if (inst.Shape.IsValid())
						{
							shapeList.RemoveAll((MyObjectBuilder_FractureComponentBase.FracturedShape s) => s.Name == inst.ShapeName);
						}
					}
					item = new MyObjectBuilder_FractureComponentBase.FracturedShape
					{
						Name = name,
						Fixed = breakableShape.IsFixed()
					};
					shapeList.Add(item);
				}
				return flag2;
			}
			if (!flag)
			{
				item = new MyObjectBuilder_FractureComponentBase.FracturedShape
				{
					Name = name,
					Fixed = breakableShape.IsFixed()
				};
				shapeList.Add(item);
				return true;
			}
			return false;
		}

		protected abstract void RecreateShape(List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList);

		protected void SerializeInternal(MyObjectBuilder_FractureComponentBase ob)
		{
			MyObjectBuilder_FractureComponentBase.FracturedShape fracturedShape;
			if (string.IsNullOrEmpty(Shape.Name) || Shape.IsCompound() || Shape.GetChildrenCount() > 0)
			{
				Shape.GetChildren(m_tmpChildren);
				foreach (HkdShapeInstanceInfo tmpChild in m_tmpChildren)
				{
					fracturedShape = default(MyObjectBuilder_FractureComponentBase.FracturedShape);
					fracturedShape.Name = tmpChild.ShapeName;
					fracturedShape.Fixed = MyDestructionHelper.IsFixed(tmpChild.Shape);
					MyObjectBuilder_FractureComponentBase.FracturedShape item = fracturedShape;
					ob.Shapes.Add(item);
				}
				m_tmpChildren.Clear();
			}
			else
			{
				List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapes = ob.Shapes;
				fracturedShape = new MyObjectBuilder_FractureComponentBase.FracturedShape
				{
					Name = Shape.Name
				};
				shapes.Add(fracturedShape);
			}
		}

		public virtual void SetShape(HkdBreakableShape shape, bool compound)
		{
			if (Shape.IsValid())
			{
				Shape.RemoveReference();
			}
			Shape = shape;
			MyRenderComponentFracturedPiece myRenderComponentFracturedPiece = base.Entity.Render as MyRenderComponentFracturedPiece;
			if (myRenderComponentFracturedPiece != null)
			{
				myRenderComponentFracturedPiece.ClearModels();
				if (compound)
				{
					shape.GetChildren(m_tmpChildren);
					foreach (HkdShapeInstanceInfo tmpChild in m_tmpChildren)
					{
						if (tmpChild.IsValid())
						{
							myRenderComponentFracturedPiece.AddPiece(tmpChild.ShapeName, Matrix.Identity);
						}
					}
					m_tmpChildren.Clear();
				}
				else
				{
					myRenderComponentFracturedPiece.AddPiece(shape.Name, Matrix.Identity);
				}
				myRenderComponentFracturedPiece.UpdateRenderObject(visible: true);
			}
		}
	}
}
