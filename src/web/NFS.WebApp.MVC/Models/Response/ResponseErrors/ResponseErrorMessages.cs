namespace NFS.WebApp.MVC.Models.Response.ResponseErrors
{
    public class ResponseErrorMessages
    {
        public ResponseErrorMessages() 
        {
            Mensagens = new List<String>();
        
        }
        public List<string> Mensagens { get; set; }
    }
}