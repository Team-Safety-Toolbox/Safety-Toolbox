using Safety_Toolbox.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox.Templates
{
    public class CertTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NotExpiring { get; set; }
        public DataTemplate ExpiringSoon { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if((((CertificationData)item).ExpiryDate.Year == DateTime.Today.Year) && (((CertificationData)item).ExpiryDate.Month == DateTime.Today.Month)){
                return ExpiringSoon;
            }
            else{
                return NotExpiring;
            }
            
        }
        
    }
}
