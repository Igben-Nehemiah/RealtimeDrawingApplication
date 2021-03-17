using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Ioc;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class MenuPaneViewModel : BindableBase
    {
        public CreateProjectWindowView createProjectWindowView;
        public CreateProjectWindowViewModel _createProjectWindowViewModel;
        public ShareProjectWindowView shareProjectWindowView;
        public ShareProjectWindowViewModel _shareProjectWindowViewModel;

        //Create Project view
        private string _projectName = "No Name";
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
            }
        }


        //Commands
        public DelegateCommand ShowCreateProjectWindowCommand { get; set; }
        public DelegateCommand ShowShareProjectWindowCommand { get; set; }
        public DelegateCommand OpenProjectCommand { get; set; }
        public DelegateCommand SaveProjectCommand { get; set; }
        public DelegateCommand DeleteProjectCommand { get; set; }
        public DelegateCommand ImportProjectCommand { get; set; }
        public DelegateCommand ExportProjectCommand { get; set; }
        public DelegateCommand<string> ShowImportExportOptionsCommand { get; set; }
        public DelegateCommand SignOutCommand { get; set; }
        public DelegateCommand CloseMenuCommand { get; set; }


        IEventAggregator _eventAggregator;

        public MenuPaneViewModel()
        {
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();

            _eventAggregator.GetEvent<ShowAddSharedUserEvent>().Subscribe(ShowShareProjectWindow);
            ShowCreateProjectWindowCommand = new DelegateCommand(ShowCreateProjectWindow, () => true);
            ShowShareProjectWindowCommand = new DelegateCommand(ShowShareProjectWindow, () => true);
            OpenProjectCommand = new DelegateCommand(ExecuteOpenProject, CanExecuteOpenProject);
            SaveProjectCommand = new DelegateCommand(ExecuteSaveProject, CanExecuteSaveProject);
            DeleteProjectCommand = new DelegateCommand(ExecuteDeleteProject, CanExecuteDeleteProject);
            ShowImportExportOptionsCommand = new DelegateCommand<string>(ShowImportExportOptions, CanShowImportExportOptions);
            SignOutCommand = new DelegateCommand(SignOut, () => true);
            CloseMenuCommand = new DelegateCommand(CloseMenu, () => true);

        }

        private void CloseMenu()
        {
            _eventAggregator.GetEvent<CloseMenuBtnClickEvent>().Publish();
        }

        private void SignOut()
        {
            _eventAggregator.GetEvent<SignOutBtnClickEvent>().Publish();
        }

        //Resposible for showing pop-up
        private bool CanShowImportExportOptions(string option)
        {
            return true;
        }

        private void ShowImportExportOptions(string option)
        {
            _eventAggregator.GetEvent<ShowPopupEvent>().Publish(option);
        }

        //Delete Project
        private bool CanExecuteDeleteProject()
        {
            return true;
        }

        private void ExecuteDeleteProject()
        {
            _eventAggregator.GetEvent<DeleteProjectBtnClickEvent>().Publish();
        }


        //Save Project
        private bool CanExecuteSaveProject()
        {
            return true;
        }

        private void ExecuteSaveProject()
        {
            _eventAggregator.GetEvent<SaveProjectEvent>().Publish();
        }

        //Open Project
        private bool CanExecuteOpenProject()
        {
            return true;
        }

        private void ExecuteOpenProject()
        {
            _eventAggregator.GetEvent<OpenProjectBtnClickEvent>().Publish();
        }

        public void ShowShareProjectWindow()
        {
            shareProjectWindowView = new ShareProjectWindowView();
            _shareProjectWindowViewModel = new ShareProjectWindowViewModel(_eventAggregator);
            //shareProjectWindowView.DataContext = _shareProjectWindowViewModel;
            shareProjectWindowView.ShowDialog();
        }

        //Share Project
        private void ShowCreateProjectWindow()
        {

            createProjectWindowView = new CreateProjectWindowView();
            _createProjectWindowViewModel = new CreateProjectWindowViewModel(_eventAggregator);
           
            createProjectWindowView.DataContext = _createProjectWindowViewModel;
            createProjectWindowView.ShowDialog();
        }

    }
}
