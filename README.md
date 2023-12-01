# dotnet-test

There are two dotnet project under the src directory, `projectA`, `projectB`.
Projects are build with Nuke and a docker image will be created for each.
Nuke will be able to identify file changes of a project and only build that project.
A github action was setup with nuke, that will trigger nuke and built the images after each merge.