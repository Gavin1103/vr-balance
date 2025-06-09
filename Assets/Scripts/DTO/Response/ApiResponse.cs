namespace DTO.Response
{
    public class ApiResponse<T>
    {
        public int status;
        public string message;
        public string timestamp;
        public T data;
    }
}