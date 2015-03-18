# YµP(Why micro Pascal?)-Compiler Project
Micro Pascal compiler final project for Compilers class at Montana State University. Team members include: Jesse Brown, Brian Maher, and Sean Rogers.

## C# Compiler
This project uses the mono C# compiler, available at http://www.mono-project.com/

To compile: `mcs -out:YµP.exe *.cs`

To run: `mono YµP.exe <µPascal file> <output filename>`

### To compile with Vagrant
If you find that it is frustrating install the mono compiler, you may wish to use our vagrant setup. This creates a virtual machine set to Ubuntu Trusty Tahr, with the mono complete installation.

- Install vagrant on your machine as per instructions on the [Vagrant website](https://www.vagrantup.com/downloads.html)
- run `vagrant up` from the root directory
- run `vagrant ssh` when vagrant has finished provisioning
- (within vagrant) `cd /vagrant`
- follow compile steps above


### Style ALL the things

Set your editor up to recognize .editorconfig, to maintain style settings.
Instructions can be found at [EditorConfig](http://editorconfig.org)

A list of editors known to support .editorconfig files can be found
at: http://editorconfig.org/#download
