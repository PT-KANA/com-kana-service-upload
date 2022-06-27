using Com.Kana.Service.Upload.Lib.Models.GarmentInternNoteModel;
using Com.Kana.Service.Upload.Lib.Models.UnitPaymentOrderModel;

namespace Com.Kana.Service.Upload.Lib.Facades.VBRequestPOExternal
{
    public class SupplierDto
    {

        public SupplierDto(UnitPaymentOrder element)
        {
            int.TryParse(element.SupplierId, out var id);
            Id = id;
            Code = element.SupplierCode;
            Name = element.SupplierName;
        }

        public SupplierDto(GarmentInternNote element)
        {
            Id = (int)element.SupplierId.GetValueOrDefault();
            Code = element.SupplierCode;
            Name = element.SupplierName;
        }

        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
    }
}