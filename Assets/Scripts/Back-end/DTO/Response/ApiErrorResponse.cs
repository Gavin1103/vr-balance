namespace DTO.Response
{
    public class ApiErrorResponse<T>
    {
        public int status;
        public string error;
        public string message;
        public string timestamp;
        public T data;
    }
    
    public class Void { }

}