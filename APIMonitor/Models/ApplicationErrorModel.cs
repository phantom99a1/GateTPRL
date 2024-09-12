using CommonLib;

namespace APIMonitor.Models
{
    public class ApplicationErrorModel
    {
        public List<ApplicationError> ListAllErrors { get; set; } = new();

        public List<ApplicationError> ListDisplayErrors { get; set; } = new();

        public int? PageIndexApplicationError { get; set; }

        public int? PageIndexMaxpplicationError { get; set; }
    }
}
