
namespace Server.Misc
{
    public class ItemUtility
    {
        public static string GetItemName(Item item)
        {
            if (item == null) return "";
            if (item.Name != null)
            {
                return item.Name;
            }
            return item.GetType().Name;
        }
    }
}
