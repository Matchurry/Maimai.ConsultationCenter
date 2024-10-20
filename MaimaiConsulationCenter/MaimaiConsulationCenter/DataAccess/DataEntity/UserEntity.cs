namespace MaimaiConsulationCenter.DataAccess.DataEntity
{
    public class UserEntity
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public int Gender { get; set; }
        public string Cookie { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
