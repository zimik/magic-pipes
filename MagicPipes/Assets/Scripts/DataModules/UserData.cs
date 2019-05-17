public interface IUserData
{
    int Record { set; get; }
}

public class UserData : IUserData
{
    public int Record { set; get; }
}
