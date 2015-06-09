namespace tapStoryWebApi.Accounts.DTO
{
    public class UserInfoModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

}