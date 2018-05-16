# Templates
## Visual Studio Templates

### Preparation

### Add a custom template
1) Add substitution parameters according to https://docs.microsoft.com/en-us/visualstudio/ide/template-parameters
2) Export the template in Visual Studio via "Project -> Export Template" without checking "Automatically import the template to Visual Studio" 
3) Expand and modify the generated VS template
4) Make sure "Options -> Projects and Solutions -> Locations" points to the proper root folder
5) The folder structure below the root folder reflects the folder structure of the new project template dialog
6) Copy the template folder from step 3 to the appropriate folder
7) The new template should show up in the new project dialog

### Remove a custom template
1) Remove the folder from the custom template location

Optionally delete the expanded template folder from the ProjectTemplatesCache located at %APPDATA%\Microsoft\VisualStudio\<version_and_optional_ID>\ProjectTemplatesCache

### How to create Multi-Project Templates in Visual Studio

Based on https://docs.microsoft.com/en-us/visualstudio/ide/how-to-create-multi-project-templates

1) Export all required templates individually using "Visual Studio -> Project -> Export Template"
2) Extract all exported projects and delete the original zip files
3) Copy the root .vstemplate file to the folder and adjust if necessary
4) Zip the entire structure and move the resulting file to  "Documents\Visual Studio 2017\Templates\ProjectTemplates\Visual C#"
5) Open the "Developer Command Prompt for VS2017" and run "devenv /updateconfiguration"
6) Create a new project using the template
7) Open the "Package Manager Console" and run "Update-Package -Reinstall" to restore missing packages.
