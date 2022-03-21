# Plugin Alliance (PA) Changelog Analyzer

**Tired of not knowing exactly which Plugin Alliance plugins got any updates?**

This is a small, simple command line utility that helps identifying changes in each PA change logs.

Change logs are extracted from https://www.plugin-alliance.com/en/support.html, _Plugin Update History_ section.

Steps:

1. The first time, run the tool and let it initialize the database (DB) with all the changelogs from all plugins.
2. At a later time, when you want to check if there was any changes, run the tool without initializing the database. It will then compare the changelogs from the PA website with the ones saved previously in the DB.

You see? Simple, isn't it? No magic. No tricks. Just automation...