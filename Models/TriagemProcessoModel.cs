using System.ComponentModel.DataAnnotations;

namespace Protocolo_API.Model
{
    public class TriagemProcessoModel
    {
        public int Tgm_Pro_Id { get; set; }
        public int? Tgm_Mot_Id { get; set; }
        public int? Tgm_resul_id { get; set; }
        public string? Tgm_Usuario_Operador { get; set; }

    }
}
