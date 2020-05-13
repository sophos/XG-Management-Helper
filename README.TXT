XG Management Helper
Created By: Alan Toews (alan.toews@sophos.com)
Created April 30, 2020
==============================
XG Management Helper is a tool to automate several administrative XG Firewall tasks

=== FEATURES ===
Basics:
Common Shell Password
If many of your firewalls use the same shell password, then you can enter it in the Common Shell Password field, for convenience, rather than entering if for each firewall being added.

Adding firewalls
On the Edit menu, select Add Firewall(s) to add new firewalls manually. You will need to enter at least the hostname or IP address for the firewall. If the firewall uses the password set in Common Shell Password, then there is no need to set a password when adding hte firewall. Clicking test will verify whether the address is accessible, and the credentials are correct. It will also retrieve the ssh key (device fingerprint) for you to review.

Import firewall list
Import expects a simple text file with one firewall IP or hostname per line. If a unique password is required for each firewall, it may be added following the firewall address, separated by a space. Do not use quotes or other characters to wrap passwords. 
Example:
	192.168.1.1 thisismypassword
	192.168.1.2
	192.168.1.3 another_pa$$w0rD_with spaces in_it

In the above example, three firewalls would be imported, and passwords set for two of them.


Available Actions:
Migrate to Sophos Central & Enable CM + CFR
 You will need to enter a valid set of credentials for Sophos Central, and this application will connect to the firewall, then register it to Sophos Central using the supplied credentials. If that is successful, it will disable SFM or CFM management (if enabled), and enable Sophos Central management and reporting. you will then need to login to Sophos Central and approve the firewall and addit into a firewall group, if desired. You will be warned before the process begins. 
 
Register to Sophos Central and Enable Central Management Only
 This action will be the same as hte previous, but will only enable firewall management, but not reporting. You will be warned before the process begins. 

Register to Sophos Central and Enable Central Firewall Reporting Only
 This action will be the same as hte previous, but will only enable firewall reporting, but not management. You will be warned before the process begins. 
 
De-register from Sophos Central
 This action will de-register selected firewalls from Sophos Central. this wil disable firewall management and reporting, if they are enabled. You will be warned before the process begins. 
 
Bulk change "admin" Password
 This action gives you two choices. 
 1) Generate a random password for each selected firewall. This is most secure, and will record the changed password for you to lookup later, if needed. 
 2) Choose a single password to apply to each selected firewall. This is not generally recommended, but the application will help you choose a secure password. 
 In both cases, the application will run through each selected firewall, and change the password as configured. The attempted password will be written to an encrypted logfile before the attempt, then the results will be written after it completes. Encrypted logs can be viewed and managed from the View menu. The firewall record in the application will be updated with the new password, once the process is complete. 
 
Check Current Firmware Version
Install Any Available Hotfixes
 If automatic hotfix installation is disabled, this app can trigger available hotfixes to download and install. You will receive confirmation that the command completed successfully, but it will not report whether any hotfixes were available to install, or whether they installed successfuly. You may use the "Check Current Firmware Version" action to check again later, whether hotfixes may have bene installed or not.

=== WARNINGS ===
Every effort was made to ensure this application will operate with minimal risk to the user. However, 
changing the admin password through an automated solution carries some risk. The application attempts 
to account for any possible failures by logging the attempted password before the change, and the results after, 
but if any unforseen disruption prevents the application from successfully recording and displaying 
the pasword used, then you may be left with no record of the admin password used. Admin account passwords 
can be reset with physical access to the system, during a reboot of hte appliance. However, the safest and
easiest recovery method is to ensure the firewall is manged by Sophos Central. This will allow you to easily reset
the admin password even if unforseen events occur.

=== SECURITY ===
Accessing firewalls: This application uses SSH to access firewalls. You should avoid exposing SSH broadly to the internet, or untrusted networks. It is recommended that you use access control rules to limit access to SSH as precisely as possible, or access the firewall over secure VPN tunnels. 

SSH establishes trust on first contact. This application will default to trusting the SSH key first presented by a firewall, and will only alert you if the key changes in future. You can check hte stored key at any time by double-clicking on a listed firewall. If you wish to interactively approve all initial connections, then disable "Trust Initial SSH Fingerprint" on the Edit menu. 

Storing passwords: This application stores sensitive information securely by using strong encryption using a key derived from a user specified password. The registry keys used are protected, and access is limited to the current user. If this password is lost, you will not be able to decrypt stored passwords, unless you have backed up the application key. You will be prompted to do this on first launch, or you can select it at any time on the file menu. Be sure to store the recovery file it creates, in a secure location, as it can be used to bypass the password you have set for your installation. 









