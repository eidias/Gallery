## Visual Studio Templates
### How to create Multi-Project Templates in Visual Studio

Based on https://docs.microsoft.com/en-us/visualstudio/ide/how-to-create-multi-project-templates

1) Export all required templates individually using "Visual Studio -> Project -> Export Template"
2) Extract all exported projects and delete the original zip files
3) Copy the root .vstemplate file to the folder and adjust if necessary
4) Zip the entire structure and move the resulting file to  "Documents\Visual Studio 2017\Templates\ProjectTemplates\Visual C#"
5) Open the "Developer Command Prompt for VS2017" and run "devenv /updateconfiguration"
6) Create a new project using the template
7) Open the "Package Manager Console" and run "Update-Package -Reinstall" to restore missing packages.
