namespace Protocolo_web_adm.Models
{
    public class MenuModel
    {
        public int MP_id { get; set; }
        public string MP_Descricao { get; set; }
        public List<SubMenuModel> SubMenus { get; set; }
    }

    public class SubMenuModel
    {
        public string? MF_Descricao { get; set; }
        public string? MF_URL { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
    }
}
