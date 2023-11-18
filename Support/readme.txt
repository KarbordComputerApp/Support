دسترسی به ورد

In the command line put DCOMCNFG
Component Services -> Computers -> My Computer -> DCOM Config
Find "Microsoft Word 97 - 2003 Document" (If it is missing check if your Word is also 64 bit (if your Windows is) if it is not run mmc comexp.msc /32 instead of DCOMCNFG on step 1 as suggested by Darkseal here)
Right click -> Properties
Go To Tab Security and Edit the "Customize" radio buttons so that IIS_IUSRS could have rights for launch and access
Go to Tab Identity and choose "The interactive user"
Apply changes and try again
If all this fails, go also to tab "General" and in "Authentication Level" drop down choose "None".