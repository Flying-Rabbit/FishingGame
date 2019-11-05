public class GameData
{
    private static GameData instance;
    public GameData()
    {
        TotalExp = 0;
        Title = "新手";
        BigTimer = 0;
        LittleTimer = 0;
        Coin = 500;
    }

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }

            return instance;
        }
    }

    public int TotalExp;
    public string Title;
    public int BigTimer;
    public int LittleTimer;
    public int Coin;
}
