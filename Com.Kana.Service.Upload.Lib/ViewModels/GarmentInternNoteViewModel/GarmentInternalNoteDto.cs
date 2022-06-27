using Com.Kana.Service.Upload.Lib.Models.GarmentInternNoteModel;
using Com.Kana.Service.Upload.Lib.Models.GarmentInvoiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.GarmentInternNoteViewModel
{
    public class GarmentInternalNoteDto
    {
        public GarmentInternalNoteDto(int internalNoteId, string internalNoteNo, DateTimeOffset internalNoteDate, DateTimeOffset internalNoteDueDate, int supplierId, string supplierName, string supplierCode, int currencyId, string currencyCode, double currencyRate, List<InternalNoteInvoiceDto> internalNoteInvoices)
        {
            InternalNote = new InternalNoteDto(internalNoteId, internalNoteNo, internalNoteDate, internalNoteDueDate, supplierId, supplierName, supplierCode, currencyId, currencyCode, currencyRate, internalNoteInvoices);
        }

        public InternalNoteDto InternalNote { get; set; }

        
    }
}
