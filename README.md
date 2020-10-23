# besucherparken-hamburg
Small application to get a parking ticket for the German city of Hamburg's Besucherparken service more easily.
This is really just a proof-of-concept, used by me personally. 
It does not do any complex input verification and is prone to break when the Besucherparken website changes.

Tech: C# (simple GUI with WPF), Selenium (using Firefox WebDriver)


## Usage instructions

* Clone repository
* Compile / Deploy Backend and GUI or Shell
* Edit dll.config files to enter your own address and account data
* Download current geckodriver executable and put it in your bin folder
* Run GUI or run via command-line
