namespace WindowsFormsApp3
{
    internal class MySqlCommand
    {
        private string selectQuery4;
        private MySqlConnection con;

        public MySqlCommand(string selectQuery4, MySqlConnection con)
        {
            this.selectQuery4 = selectQuery4;
            this.con = con;
        }
    }
}