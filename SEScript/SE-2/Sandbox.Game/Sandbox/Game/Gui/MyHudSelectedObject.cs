using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRageMath;
using VRageRender.Models;

namespace Sandbox.Game.Gui
{
	public class MyHudSelectedObject
	{
		[ThreadStatic]
		private static List<string> m_tmpSectionNames = new List<string>();

		[ThreadStatic]
		private static List<uint> m_tmpSubpartIds = new List<uint>();

		private bool m_highlightAttributeDirty;

		private bool m_visible;

		private uint m_visibleRenderID = uint.MaxValue;

		private string m_highlightAttribute;

		internal MyHudSelectedObjectStatus CurrentObject;

		internal MyHudSelectedObjectStatus PreviousObject;

		private Vector2 m_halfSize = Vector2.One * 0.02f;

		private Color m_color = MyHudConstants.HUD_COLOR_LIGHT;

		private MyHudObjectHighlightStyle m_style;

		internal MyHudSelectedObjectState State
		{
			get;
			private set;
		}

		public string HighlightAttribute
		{
			get
			{
				return m_highlightAttribute;
			}
			internal set
			{
				if (!(m_highlightAttribute == value))
				{
					CheckForTransition();
					m_highlightAttribute = value;
					CurrentObject.SectionNames = null;
					CurrentObject.SubpartIndices = null;
					if (value != null)
					{
						m_highlightAttributeDirty = true;
					}
				}
			}
		}

		public MyHudObjectHighlightStyle HighlightStyle
		{
			get
			{
				return m_style;
			}
			set
			{
				if (m_style != value)
				{
					CheckForTransition();
					m_style = value;
				}
			}
		}

		public Vector2 HalfSize
		{
			get
			{
				return m_halfSize;
			}
			set
			{
				if (!(m_halfSize == value))
				{
					CheckForTransition();
					m_halfSize = value;
				}
			}
		}

		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				if (!(m_color == value))
				{
					CheckForTransition();
					m_color = value;
				}
			}
		}

		public bool Visible
		{
			get
			{
				return m_visible;
			}
			internal set
			{
				if (value)
				{
					m_visibleRenderID = CurrentObject.Instance.RenderObjectID;
				}
				else
				{
					m_visibleRenderID = uint.MaxValue;
				}
				if (value)
				{
					CurrentObject.Style = m_style;
				}
				else
				{
					CurrentObject.Style = MyHudObjectHighlightStyle.None;
				}
				m_visible = value;
				State = MyHudSelectedObjectState.VisibleStateSet;
			}
		}

		public uint VisibleRenderID => m_visibleRenderID;

		public IMyUseObject InteractiveObject => CurrentObject.Instance;

		internal uint[] SubpartIndices
		{
			get
			{
				ComputeHighlightIndices();
				return CurrentObject.SubpartIndices;
			}
		}

		internal string[] SectionNames
		{
			get
			{
				ComputeHighlightIndices();
				return CurrentObject.SectionNames;
			}
		}

		public void Clean()
		{
			CurrentObject = default(MyHudSelectedObjectStatus);
			PreviousObject = default(MyHudSelectedObjectStatus);
		}

		private void ComputeHighlightIndices()
		{
			if (!m_highlightAttributeDirty)
			{
				return;
			}
			if (m_highlightAttribute == null)
			{
				m_highlightAttributeDirty = false;
				return;
			}
			m_tmpSectionNames.Clear();
			m_tmpSubpartIds.Clear();
			string[] array = m_highlightAttribute.Split(new char[1]
			{
				";"[0]
			});
			MyModel model = CurrentObject.Instance.Owner.Render.GetModel();
			bool flag = true;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.StartsWith("subpart_"))
				{
					string name = text.Substring("subpart_".Length);
					flag = CurrentObject.Instance.Owner.TryGetSubpart(name, out MyEntitySubpart subpart);
					if (!flag)
					{
						break;
					}
					uint renderObjectID = subpart.Render.GetRenderObjectID();
					if (renderObjectID != uint.MaxValue)
					{
						m_tmpSubpartIds.Add(renderObjectID);
					}
				}
				else if (text.StartsWith("subblock_"))
				{
					MyCubeBlock myCubeBlock = CurrentObject.Instance.Owner as MyCubeBlock;
					if (myCubeBlock == null)
					{
						break;
					}
					string name2 = text.Substring("subblock_".Length);
					flag = myCubeBlock.TryGetSubBlock(name2, out MySlimBlock block);
					if (!flag)
					{
						break;
					}
					uint renderObjectID2 = block.FatBlock.Render.GetRenderObjectID();
					if (renderObjectID2 != uint.MaxValue)
					{
						m_tmpSubpartIds.Add(renderObjectID2);
					}
				}
				else
				{
					flag = model.TryGetMeshSection(array[i], out MyMeshSection section);
					if (!flag)
					{
						break;
					}
					m_tmpSectionNames.Add(section.Name);
				}
			}
			if (flag)
			{
				CurrentObject.SectionNames = m_tmpSectionNames.ToArray();
				if (m_tmpSubpartIds.Count != 0)
				{
					CurrentObject.SubpartIndices = m_tmpSubpartIds.ToArray();
				}
			}
			else
			{
				CurrentObject.SectionNames = new string[0];
				CurrentObject.SubpartIndices = null;
			}
			m_highlightAttributeDirty = false;
		}

		internal void Highlight(IMyUseObject obj)
		{
			if (SetObjectInternal(obj))
			{
				return;
			}
			if (m_visible)
			{
				if (State == MyHudSelectedObjectState.MarkedForNotVisible)
				{
					State = MyHudSelectedObjectState.VisibleStateSet;
				}
			}
			else
			{
				State = MyHudSelectedObjectState.MarkedForVisible;
			}
		}

		internal void RemoveHighlight()
		{
			if (m_visible)
			{
				State = MyHudSelectedObjectState.MarkedForNotVisible;
			}
			else if (State == MyHudSelectedObjectState.MarkedForVisible)
			{
				State = MyHudSelectedObjectState.VisibleStateSet;
			}
		}

		internal void ResetCurrent()
		{
			CurrentObject.Reset();
			m_highlightAttributeDirty = true;
		}

		private bool SetObjectInternal(IMyUseObject obj)
		{
			if (CurrentObject.Instance == obj)
			{
				return false;
			}
			bool result = CheckForTransition();
			ResetCurrent();
			CurrentObject.Instance = obj;
			return result;
		}

		private bool CheckForTransition()
		{
			if (CurrentObject.Instance == null || !m_visible)
			{
				return false;
			}
			if (PreviousObject.Instance != null)
			{
				return true;
			}
			DoTransition();
			return true;
		}

		private void DoTransition()
		{
			PreviousObject = CurrentObject;
			State = MyHudSelectedObjectState.MarkedForVisible;
		}
	}
}
