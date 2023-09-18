using Dapper;
using Protocolo_web_adm.Models;
using Protocolo_web_adm.Service.IRepository;
using System.Data;

namespace Protocolo_web_adm.Service.Repository
{
    public class MenuService : IMenuService
    {
        private readonly IDapperServices _dapper;

        public MenuService(IDapperServices dapper)
        {
            _dapper = dapper;
        }
        public List<MenuModel> ListaMenuPai(string email)
        {
            List<MenuModel> model = new List<MenuModel>();
            try
            {
                var query = @"select Distinct MP_id, MP_Descricao
                                 from MenuPai inner join MenuFilho on(MP_id = MF_MP_Id)
                                             inner join Usuario_Menu  on (MF_id = Usu_Menu_MF_Id)
											 
                                             inner join GR_APCI_BA_Salvador.dbo.APIAutenticacao on (Usu_Menu_Usu_id  = Autid)
                               where AutEmail = @email";

                var dbParametro = new DynamicParameters();
                dbParametro.Add("@email", email);

                model = _dapper.GetAll<MenuModel>(query, dbParametro, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }

        public List<SubMenuModel> ListaMenuFilho(int id, string email)
        {
            List<SubMenuModel> menusFilho = new List<SubMenuModel>();

            try
            {
                var query = @"select MF_Descricao, ControllerName,ActionName 
					      from MenuPai inner join MenuFilho on(MP_id = MF_MP_Id)
                                             inner join Usuario_Menu  on (MF_id = Usu_Menu_MF_Id)
											
                                             inner join GR_APCI_BA_Salvador.dbo.APIAutenticacao on (Usu_Menu_Usu_id  = Autid)
                      where MP_Id = @id and AutEmail = @email";

                var dbParametro = new DynamicParameters();
                dbParametro.Add("@id", id);
                dbParametro.Add("@email", email);

                menusFilho = _dapper.GetAll<SubMenuModel>(query, dbParametro, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }

            return menusFilho;
        }



    }
}
