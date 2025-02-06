using Prism.Events;

namespace WpfApp3.Events
{
    //true:编辑角色，false:新建角色
    public class ManageRolesEvent : PubSubEvent<bool>
	{
	}
}
