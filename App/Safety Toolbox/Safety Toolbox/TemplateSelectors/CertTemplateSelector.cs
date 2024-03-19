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
        // also need odd even selector
        public DataTemplate NotExpiring { get; set; }
        public DataTemplate ExpiringSoon { get; set; }
        public DataTemplate Expired { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((CertificationData)item).ExpiryDate.HasValue)
            {
                if ((((CertificationData)item).ExpiryDate.Value.Year == DateTime.Today.Year) && (((CertificationData)item).ExpiryDate.Value.Month == DateTime.Today.Month) && ((CertificationData)item).ExpiryDate.Value.Day > DateTime.Today.Day)
                {
                    return ExpiringSoon;
                }
                else if (((CertificationData)item).ExpiryDate <= DateTime.Today)
                {
                    return Expired;
                }
                else
                {
                    return NotExpiring;
                }
            }
            
            return NotExpiring;
        }
        
    }
}
