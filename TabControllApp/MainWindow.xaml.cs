using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TabControllApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region D&D
        // Drag&Drop 
        private void TabItem_Drag(object sender, MouseEventArgs e)
        {           
            var tabItem = e.Source as TabItem;

            if (tabItem == null)
                return;

            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
                DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
        }

        private TabItem GetTargetTabItem(object originalSource)
        {
            var current = originalSource as DependencyObject;

            while (current != null)
            {
                var tabItem = current as TabItem;
                if (tabItem != null)
                {
                    return tabItem;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }
                
        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItemTarget = GetTargetTabItem(e.OriginalSource);
            if (tabItemTarget != null)
            {
                var tabItemSource = (TabItem)e.Data.GetData(typeof(TabItem));
                if (tabItemTarget != tabItemSource)
                {
                    int sourceIndex = tabs.Items.IndexOf(tabItemSource);
                    int targetIndex = tabs.Items.IndexOf(tabItemTarget);

                    tabs.Items.Remove(tabItemSource);
                    tabs.Items.Insert(targetIndex, tabItemSource);

                    tabs.Items.Remove(tabItemTarget);
                    tabs.Items.Insert(sourceIndex, tabItemTarget);

                    tabs.SelectedIndex = targetIndex;
                }
            }
        }
        #endregion
        #region RemoveTab
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Определяем в каком табе нажата кнопка и удаляем его
            var tabItem = GetTargetTabItem(e.OriginalSource);
            tabs.Items.Remove(tabItem);
            // Выравнивание табов
            TabsFill();
        }
        #endregion
        #region SizeTabControl
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TabsFill();
        }

        private void TabsFill()
        {
            foreach (TabItem item in tabs.Items)
            {
                // Отступ 20 выбирался путём научного тыка
                // Делим количество табов на ширину StackPanel, 
                // отнимаем некоторый шаг, получаем нужный размер
                double wd = (RenderSize.Width / tabs.Items.Count) - 20;
                if (wd < 20) wd = 20;
                item.Width = wd;
            }
        }
        #endregion
    }
}
