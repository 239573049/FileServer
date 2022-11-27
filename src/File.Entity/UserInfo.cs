namespace File.Entity
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar  { get; set; }

        public UserInfo(string username, string password, string avatar)
        {
            Id=Guid.NewGuid();
            Username = username;
            Password = password;
            Avatar = avatar;
        }

        protected UserInfo()
        {
            
        }
    }
}

