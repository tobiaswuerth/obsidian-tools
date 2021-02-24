# obsidian-tools
a CLI application for
- markdown manipulation
- for the [Obsidian.md](https://obsidian.md/) vault
- written with .net core

> ### You can find [the latest release by clicking here](https://github.com/tobiaswuerth/obsidian-tools/releases)!
> #### Feel free to contribute or submit ideas! You can get in contact with me via [fooo.ooo](https://fooo.ooo/)

**Please be careful!**
These tools are under construction and may cause loss of data.
Make sure you have a backup of your vault before executing any commands!

# Help Overview
```
PS >.\ch.wuerth.tobias.ObsidianTools.exe
Usage: exe <vault-path> <plugin-name> [<additional1>, ...]
Plugins:
        --count                 Counts all markdown files
        --analyze               Analyze markdown files and create word report
        --find-references       Find all files referencing the given keywords
        --create-references     Create file reference for all unlinked mentions
        --cleanup               Cleanup dead links
        --cleanup-assets        Move all unreferenced [jpg|jpeg|pdf|png] files into subfolder .\_UNUSED\
        --identify-hotspots     Find the top nodes which are most often linked to
        --list-dead             List all dead links
        --create-dead           Create all dead link files
        --reduce-noise          (beta) Create a tree-like structure starting from a given entry point
```

---

# Examples
Some example for the individuel plugins

### Count
Counts all the `.md` files in the directory, including sub-directories

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --count
```

Example output:
```
Starting plugin: Counts all markdown files
===========================================
Found 383 markdown files in given directory
===========================================
Finished plugin: Counts all markdown files
```

---

### Analyze
Analyzes the words of all the `.md` files in the directory, including sub-directories.
Counts occurrences, looks for similar terms and finds references in other files.
This can help you get insights into your knowledge.

> **Note:** This produces a large output file

**ToDo:**
- [ ] Word Filter / Blacklist
- [ ] Optimize Word Normalization

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --analyze
```
see also: [Example output](https://pastebin.com/Z6HM3LSY)

If you open the resulting output file in Obsidian, you should see something like this:
![Example Report](https://dl.dropboxusercontent.com/s/4ugn1kx4769h2h2/Obsidian_2020-08-05_14-49-58.png)

---

### Find References
Analyzes the words of all the `.md` files in the directory, including sub-directories.
Searches for matching keywords passed as an additional argument.
This can be very helpful if you want to create a table of contents based on certain words.

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --find-references projekt project unterfangen vorhaben
```

Example output:
```
Starting plugin: Find all files referencing the given keywords
===========================================
Starting to search for word references...
Searching for references done
Creating result file...
Creating result file done, you can find it here: obsidiantools-output-references-20200917-210340.md
===========================================
Finished plugin: Find all files referencing the given keywords
```

If you open the resulting output file in Obsidian, you should see something like this:
![Example Report](https://dl.dropboxusercontent.com/s/n2q9eht7mpkf2lv/Obsidian_2020-08-05_14-57-25.png)

---

### Create References
Analyzes the words of all the `.md` files in the directory, including sub-directories.
Searches for matching words based on the `.md` filename and link it to the document. Basically linking unlinked mentions. 
This can be very helpful if you want to cleanup your vault and improve bidirectional linking between the topics you already created.

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --create-references
```
see also: [Example output](https://pastebin.com/6CuNz1Yr)

If you open the resulting output file in Obsidian, you should see something like this:
![Example Changes](https://dl.dropboxusercontent.com/s/xkpi4224i5oautk/2020-09-17_21-21-30.gif)

---

### Cleanup
Cleanup all the `.md` files in the directory, including sub-directories.
Searches for links, validates if the destination file exists and has content, otherwise remove the dead link.
Empty files will be deleted in this process.
This can be very helpful if you have a lot of links which do not yet have a backing document.

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --cleanup
```
see also: [Example output](https://pastebin.com/zDP9MBWX)

If you compare the two files you can see the links which got removed.
Tipp: Use a version control tool like Git to track changes.
![Example Report](https://dl.dropboxusercontent.com/s/wl4t4llh4jt8kys/2020-08-05_13-38-06.gif)

---

### List Dead
List all the dead links in the `.md` files in the directory, including sub-directories.
Searches for links, validates if the destination file exists and has content, otherwise prints the dead link.
This can be very helpful if you have a lot of links which do not yet have a backing document.
This can also be used as a first step before executing `--cleanup` or `--create-dead`.

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --list-dead
```

Example Output:
```
Starting plugin: List all dead links
===========================================
Searching for dead links...
Loading files into memory...
Files loaded
Searching for dead links done
Creating result file done, you can find it here: obsidiantools-output-dead-20200917-212947.md
===========================================
Finished plugin: List all dead links
```

If you compare the two files you can see the links which got removed.
Tipp: Use a version control tool like Git to track changes.
![Example Report](https://dl.dropboxusercontent.com/s/2fy08uiuq17br90/Obsidian_2020-09-17_21-35-35.png)

---

### Create Dead
Analyzes all the `.md` files in the directory, including sub-directories.
Searches for links, validates if the destination file exists and has content, otherwise creates the destination file with a `#todo` mark for easy lookup.
This can be very helpful if you plan to add sections for topics and therefor add links which do not exist yet.

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --create-dead
```

Example Output:
```
Starting plugin: Create all dead link files
===========================================
Creating dead links...
Loading files into memory...
Files loaded
Created dead links
Creating result file done, you can find it here: obsidiantools-output-created-20200917-213829.md
Note: You might have to restart your Obsidian.md client in order to correctly index all new files
===========================================
Finished plugin: Create all dead link files
```

Tipp: Use a version control tool like Git to track changes.

![Example Report](https://dl.dropboxusercontent.com/s/9ug9tps0mjv1o0w/Obsidian_2020-09-17_21-40-40.png)
