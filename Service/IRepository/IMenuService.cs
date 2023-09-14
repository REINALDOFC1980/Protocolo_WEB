using Protocolo_web_adm.Models;
using System.ComponentModel.Design;

namespace Protocolo_web_adm.Service.IRepository
{
    public interface IMenuService
    {
        List<MenuModel> ListaMenuPai(string email);
        List<SubMenuModel> ListaMenuFilho(int id, string email);

    }
}
