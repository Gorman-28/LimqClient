namespace LimqClient.Settings;

public static class SettingArray
{
    public static MyNamespace.User? MyUser = new MyNamespace.User
    {
        Id = Guid.Parse("7891d5a0-22da-4402-912d-0bf7002b1442"),
        UserName = "Gorman",
        Password = "",
        FirstName = "Dmitry",
        LastName = "Mikhieiev",
        Avatar = new byte[0],
        Status = true
    };
    public static string[] whiteTheme = new string[]{"#FACC5F"/*0*/, "#FFFFFF" /*1*/, "rgba(0, 0, 0, 0.2)" /*2*/, "#000000" /*3*/, "rgba(255, 255, 255, 0.49)" /*4*/,
    "rgba(0, 0, 0, 0.16)" /*5*/, "#FAFAFA" /*6*/, "#FBCD60" /*7*/, "rgba(183, 183, 183, 0.15)" /*8*/, "#B38C30" /*9*/, "#B28C30" /*10*/, "#FEF7E6" /*11*/, "#FFE19A" /*12*/,
    "#BAA77B" /*13*/, "rgba(178, 130, 100, 0.4)" /*14*/}; //15

    public static string[] blackTheme = new string[1];

}
