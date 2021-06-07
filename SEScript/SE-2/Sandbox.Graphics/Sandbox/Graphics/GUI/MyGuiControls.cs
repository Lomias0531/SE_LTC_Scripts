using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using VRage.Collections;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControls : MyGuiControlBase.Friend, IEnumerable<MyGuiControlBase>, IEnumerable
	{
		private IMyGuiControlsOwner m_owner;

		private ObservableCollection<MyGuiControlBase> m_controls;

		private Dictionary<string, MyGuiControlBase> m_controlsByName;

		private List<MyGuiControlBase> m_visibleControls;

		private bool m_refreshVisibleControls;

		public int Count => m_controls.Count;

		public MyGuiControlBase this[int index]
		{
			get
			{
				return m_controls[index];
			}
			set
			{
				MyGuiControlBase myGuiControlBase = m_controls[index];
				if (myGuiControlBase != null)
				{
					myGuiControlBase.VisibleChanged -= control_VisibleChanged;
					m_visibleControls.Remove(myGuiControlBase);
				}
				if (value != null)
				{
					value.VisibleChanged -= control_VisibleChanged;
					value.VisibleChanged += control_VisibleChanged;
					m_controls[index] = value;
					m_refreshVisibleControls = true;
				}
			}
		}

		public event Action<MyGuiControls> CollectionChanged;

		public event Action<MyGuiControls> CollectionMembersVisibleChanged;

		public MyGuiControls(IMyGuiControlsOwner owner)
		{
			m_owner = owner;
			m_controls = new ObservableCollection<MyGuiControlBase>();
			m_controlsByName = new Dictionary<string, MyGuiControlBase>();
			m_visibleControls = new List<MyGuiControlBase>();
			m_controls.CollectionChanged += OnPrivateCollectionChanged;
			m_refreshVisibleControls = true;
		}

		public void Init(MyObjectBuilder_GuiControls objectBuilder)
		{
			Clear();
			if (objectBuilder.Controls != null)
			{
				foreach (MyObjectBuilder_GuiControlBase control in objectBuilder.Controls)
				{
					MyGuiControlBase myGuiControlBase = MyGuiControlsFactory.CreateGuiControl(control);
					if (myGuiControlBase != null)
					{
						myGuiControlBase.Init(control);
						Add(myGuiControlBase);
					}
				}
			}
		}

		public MyObjectBuilder_GuiControls GetObjectBuilder()
		{
			MyObjectBuilder_GuiControls myObjectBuilder_GuiControls = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GuiControls>();
			myObjectBuilder_GuiControls.Controls = new List<MyObjectBuilder_GuiControlBase>();
			foreach (KeyValuePair<string, MyGuiControlBase> item in m_controlsByName)
			{
				MyObjectBuilder_GuiControlBase objectBuilder = item.Value.GetObjectBuilder();
				myObjectBuilder_GuiControls.Controls.Add(objectBuilder);
			}
			return myObjectBuilder_GuiControls;
		}

		private void OnPrivateCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this);
			}
		}

		private void control_VisibleChanged(object control, bool isVisible)
		{
			m_refreshVisibleControls = true;
			if (this.CollectionMembersVisibleChanged != null)
			{
				this.CollectionMembersVisibleChanged(this);
			}
		}

		private void RefreshVisibleControls()
		{
			if (m_refreshVisibleControls)
			{
				m_visibleControls.Clear();
				foreach (MyGuiControlBase control in m_controls)
				{
					if (control.Visible)
					{
						m_visibleControls.Add(control);
					}
				}
				m_refreshVisibleControls = false;
			}
		}

		public List<MyGuiControlBase> GetVisibleControls()
		{
			RefreshVisibleControls();
			return m_visibleControls;
		}

		public void Add(MyGuiControlBase control)
		{
			MyGuiControlBase.Friend.SetOwner(control, m_owner);
			control.Name = ChangeToNonCollidingName(control.Name);
			m_controlsByName.Add(control.Name, control);
			if (control.Visible)
			{
				m_visibleControls.Add(control);
			}
			m_controls.Add(control);
			control.VisibleChanged += control_VisibleChanged;
			control.NameChanged += control_NameChanged;
		}

		public void AddWeak(MyGuiControlBase control)
		{
			if (control.Visible)
			{
				m_visibleControls.Add(control);
			}
			m_controls.Add(control);
			control.VisibleChanged += control_VisibleChanged;
			control.NameChanged += control_NameChanged;
		}

		private void control_NameChanged(MyGuiControlBase control, MyGuiControlBase.NameChangedArgs args)
		{
			m_controlsByName.Remove(args.OldName);
			control.NameChanged -= control_NameChanged;
			control.Name = ChangeToNonCollidingName(control.Name);
			control.NameChanged += control_NameChanged;
			m_controlsByName.Add(control.Name, control);
		}

		public void ClearWeaks()
		{
			m_controls.Clear();
			m_controlsByName.Clear();
			m_visibleControls.Clear();
		}

		public void Clear()
		{
			foreach (MyGuiControlBase control in m_controls)
			{
				control.OnRemoving();
				control.VisibleChanged -= control_VisibleChanged;
				control.NameChanged -= control_NameChanged;
			}
			m_controls.Clear();
			m_controlsByName.Clear();
			m_visibleControls.Clear();
		}

		public bool Remove(MyGuiControlBase control)
		{
			m_controlsByName.Remove(control.Name);
			bool num = m_controls.Remove(control);
			if (num)
			{
				m_visibleControls.Remove(control);
				control.OnRemoving();
				control.VisibleChanged -= control_VisibleChanged;
				control.NameChanged -= control_NameChanged;
			}
			return num;
		}

		public bool RemoveControlByName(string name)
		{
			MyGuiControlBase controlByName = GetControlByName(name);
			if (controlByName == null)
			{
				return false;
			}
			return Remove(controlByName);
		}

		public int IndexOf(MyGuiControlBase item)
		{
			return m_controls.IndexOf(item);
		}

		public int FindIndex(Predicate<MyGuiControlBase> match)
		{
			return m_controls.FindIndex(match);
		}

		public MyGuiControlBase GetControlByName(string name)
		{
			MyGuiControlBase value = null;
			m_controlsByName.TryGetValue(name, out value);
			return value;
		}

		private string ChangeToNonCollidingName(string originalName)
		{
			string text = originalName;
			int num = 1;
			while (m_controlsByName.ContainsKey(text))
			{
				text = originalName + num;
				num++;
			}
			return text;
		}

		public bool Contains(MyGuiControlBase control)
		{
			return m_controls.Contains(control);
		}

		public ObservableCollection<MyGuiControlBase>.Enumerator GetEnumerator()
		{
			return m_controls.GetEnumerator();
		}

		IEnumerator<MyGuiControlBase> IEnumerable<MyGuiControlBase>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
