namespace Lab_Project_WebApi.DbContext
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public AddressDto Address { get; set; }
    }

}
