using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BasicTest
{
    public partial class MySQLConn : Window
    {
        public MySqlConnection? myConnection;
        private readonly string myConnectionString;

        public MySQLConn()
        {
            InitializeComponent();
            // 从配置文件获取连接字符串
            myConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            myConnection = new MySqlConnection(myConnectionString);
        }

        private void GetData_Click(object sender, RoutedEventArgs e)
        {
            if (myConnection == null)
            {
                MessageBox.Show("数据库连接未初始化", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // 确保连接是关闭状态，然后再打开
                if (myConnection.State != ConnectionState.Open)
                {
                    myConnection.Open();
                }

                // 使用 using 语句确保资源正确释放
                using (MySqlCommand myCommand = new MySqlCommand())
                {
                    myCommand.Connection = myConnection;
                    myCommand.CommandText = @"SELECT * FROM books WHERE price > @price;";
                    myCommand.Parameters.AddWithValue("@price", 10);

                    using (var myReader = myCommand.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(myReader);
                        MyGrid.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"数据库错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // 确保连接关闭
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            myConnection?.Close();
        }
    }
}