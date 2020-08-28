# Versioning
Increment the version in a file. This project was specifically designed to update the image version of a k8s cloudbuild yaml file. The exe file is designed to be executed on Target Exec Command for AfterPublish.

# Source code
C# console application

# Framework
netcoreapp2.1

# Command line arguments
-file <filepath> -version <version to be searched> -incrementtype <increment type>

# Filepath argument
Filepath argument is the target file to increment the version.

# Increment types
Major
Minor
Patch

# Target Exec Command for AfterPublish sample
This sample is with assumption that the project is under the same working directory as the increment version exe file.

  <Target Name="CustomActionsAfterPublish" AfterTargets="AfterPublish">
    <Exec WorkingDirectory="D:\SamplePath" Command="increment-version.exe -file SampleProject\cloudbuild.yaml -version project-service:live- -incrementtype patch"></Exec>
  </Target>
