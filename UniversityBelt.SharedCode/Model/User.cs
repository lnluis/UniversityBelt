using System.Runtime.Serialization;

namespace UniversityBelt.SharedCode.Model
{
    public class User
    {
        public int Id { get; set; }

        [DataMember(Name = "UserId")]
        public string UserId { get; set; }
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }
        [DataMember(Name = "Age")]
        public int Age { get; set; }
        [DataMember(Name = "AccessToken")]
        public string AccessToken { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
        [DataMember(Name = "ProfilePictureUrl")]
        public string ProfilePictureUrl { get; set; }
    }
}