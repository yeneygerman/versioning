# Versioning
Increment the version in a file. This project was specifically designed to update the image version of a k8s cloudbuild yaml file. The exe file is designed to be executed on Target Exec Command for AfterPublish.

# Source code
C# console application

# Framework
netcoreapp2.1

# Command line arguments
-file "filepath" -version "version to be searched" -incrementtype "increment type"

# Filepath argument
Filepath argument is the target file to increment the version.

# Increment types
Major
Minor
Patch
