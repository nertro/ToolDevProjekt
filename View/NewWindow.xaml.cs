
namespace ToolDevProjekt.View
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using ToolDevProjekt.Control;

    public partial class NewWindow
    {
        private App controller;

        public NewWindow()
        {
            this.controller = (App)Application.Current;

            InitializeComponent();
        }

        private void NewMapButton_Click(object sender, RoutedEventArgs e)
        {
            this.controller.CreateNewMap(int.Parse(TextBoxWidth.Text), int.Parse(TextBoxHeight.Text));
        }
    }
}
