## GitHub actions and .Net
This repository serves as an example of how one can implement GitHub actions to build and test .Net projects.

Each project/solution has a dedicated action .yml file that approx follow the same pattern.

Each action is sensitive to the presence of certain sentinel strings being present in the pathnames of files present within a commit.

[![Build.Net.Scheduling](https://github.com/Korporal/actionstuff/actions/workflows/build.nep.scheduling.yml/badge.svg)](https://github.com/Korporal/actionstuff/actions/workflows/build.nep.scheduling.yml)
