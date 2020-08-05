# obsidian-tools
a CLI application for
- markdown manipulation
- for the [Obsidian.md](https://obsidian.md/) vault
- written with .net core

You can find [the latest release by clicking here](https://github.com/tobiaswuerth/obsidian-tools/releases)!

#### Feel free to contribute or submit ideas! You can get in contact with me via [fooo.ooo](https://fooo.ooo/)

> **Please be careful!**
> These tools are under construction and may cause loss of data. Make sure you have a backup of your vault before executing any commands!

## Help Overview
```
PS >.\ch.wuerth.tobias.ObsidianTools.exe
Usage: exe <vault-path> <plugin-name> [<additional1>, ...]
Plugins:
        --count             Counts all markdown files
        --analyze           Analyze markdown files and create word report
        --find-references   Find all files referencing the given keywords
        --cleanup           Cleanup dead links
```

## Examples
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
Found 281 markdown files in given directory
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

Example output:
```
Starting plugin: Analyze markdown files and create word report
===========================================
Starting to analyze words in files...
Analyzing words done
-------------
Top 10
 - Words (occurred 7440x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - --- (occurred 3766x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - Counted (occurred 3720x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - Similar (occurred 3720x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - References (occurred 3720x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - Psychology (occurred 1082x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - Projekt (occurred 908x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - und (occurred 483x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - Marketing (occurred 450x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
 - Management (occurred 407x) (0x similar: <none>, ..) @ (0x referenced: <none>, ..)
-------------
Starting to search for similar words...
Searching for similar words done
-------------
Top 10
 - ung (occurred 3x) (309x similar: Arbeitsvermittlungszentrum, Anstellung, Segmentierungskriterium, ..) @ (0x referenced: <none>, ..)
 - ich (occurred 20x) (221x similar: Fehlermöglichkeits-, Pflichtenheft, Statusbericht, ..) @ (0x referenced: <none>, ..)
 - Ich (occurred 4x) (221x similar: Fehlermöglichkeits-, Pflichtenheft, Statusbericht, ..) @ (0x referenced: <none>, ..)
 - Ein (occurred 36x) (134x similar: Einflussanalyse, eine, ein, ..) @ (0x referenced: <none>, ..)
 - ein (occurred 52x) (134x similar: Einflussanalyse, eine, ein, ..) @ (0x referenced: <none>, ..)
 - pro (occurred 5x) (97x similar: Projekt, Projektdesign, Projektmanagement, ..) @ (0x referenced: <none>, ..)
 - den (occurred 57x) (88x similar: Kundenmerkmal, den, Kundenzufriedenheit, ..) @ (0x referenced: <none>, ..)
 - Der (occurred 15x) (82x similar: der, Anforderung, Leadership, ..) @ (0x referenced: <none>, ..)
 - der (occurred 197x) (82x similar: der, Anforderung, Leadership, ..) @ (0x referenced: <none>, ..)
 - sen (occurred 4x) (74x similar: Präsenz, Arbeitslosenkasse, Applikationsentwickler, ..) @ (0x referenced: <none>, ..)
-------------
Starting to search for word references in files...
Searching for references done
-------------
Top 10
 - sie (occurred 16x) (36x similar: siehe, Realisierung, Sie, ..) @ (184x referenced: C:\myvault\3R Kundenbeziehungs-Phasen.md, C:\myvault\5-Phasen Modell.md, C:\myvault\5-Why-Methode.md, ..)
 - Sie (occurred 22x) (36x similar: siehe, Realisierung, Sie, ..) @ (184x referenced: C:\myvault\3R Kundenbeziehungs-Phasen.md, C:\myvault\5-Phasen Modell.md, C:\myvault\5-Why-Methode.md, ..)
 - siehe (occurred 207x) (1x similar: siehe, ..) @ (168x referenced: C:\myvault\3R Kundenbeziehungs-Phasen.md, C:\myvault\5-Phasen Modell.md, C:\myvault\5-Why-Methode.md, ..)
 - ung (occurred 3x) (309x similar: Arbeitsvermittlungszentrum, Anstellung, Segmentierungskriterium, ..) @ (162x referenced: C:\myvault\5-Why-Methode.md, C:\myvault\Abenteuer.md, C:\myvault\Abgrenzung.md, ..)
 - Auch (occurred 7x) (8x similar: auch, braucht, brauchen, ..) @ (147x referenced: C:\myvault\3R Kundenbeziehungs-Phasen.md, C:\myvault\5-Phasen Modell.md, C:\myvault\5-Why-Methode.md, ..)
 - auch (occurred 155x) (8x similar: auch, braucht, brauchen, ..) @ (147x referenced: C:\myvault\3R Kundenbeziehungs-Phasen.md, C:\myvault\5-Phasen Modell.md, C:\myvault\5-Why-Methode.md, ..)
 - ein (occurred 52x) (134x similar: Einflussanalyse, eine, ein, ..) @ (132x referenced: C:\myvault\5-Why-Methode.md, C:\myvault\Ablauf- und Terminplan.md, C:\myvault\Anforderung.md, ..)
 - Ein (occurred 36x) (134x similar: Einflussanalyse, eine, ein, ..) @ (132x referenced: C:\myvault\5-Why-Methode.md, C:\myvault\Ablauf- und Terminplan.md, C:\myvault\Anforderung.md, ..)
 - Der (occurred 15x) (82x similar: der, Anforderung, Leadership, ..) @ (128x referenced: C:\myvault\5-Why-Methode.md, C:\myvault\Abenteuer.md, C:\myvault\Ablauf- und Terminplan.md, ..)
 - der (occurred 197x) (82x similar: der, Anforderung, Leadership, ..) @ (128x referenced: C:\myvault\5-Why-Methode.md, C:\myvault\Abenteuer.md, C:\myvault\Ablauf- und Terminplan.md, ..)
Creating result file...
Creating result file done, you can find it here: obsidiantools-output-analyze-20200805-144734.md
===========================================
Finished plugin: Analyze markdown files and create word report
```

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
Creating result file done, you can find it here: obsidiantools-output-references-20200805-145645.md
===========================================
Finished plugin: Find all files referencing the given keywords
```

If you open the resulting output file in Obsidian, you should see something like this:
![Example Report](https://dl.dropboxusercontent.com/s/n2q9eht7mpkf2lv/Obsidian_2020-08-05_14-57-25.png)


---

### Cleanup
Cleanup all the `.md` files in the directory, including sub-directories.
Searches for links, validates if the destination file exists and has content, otherwise remove the dead link.
Empty files will be deleted in this process.
This can be very helpful if you have a lot of links which do not yet have a backing document.

**ToDo:**
- [ ] Fix Section links (e.g. `[[Anforderung#Formulierung|Anforderungsformulierung]]`)

```ps
PS >.\ch.wuerth.tobias.ObsidianTools.exe C:\myvault --cleanup
```

Example output:
```
Starting plugin: Cleanup dead links
===========================================
Starting to cleanup...
Dead link found: link 'C:\myvault\Marktabgrenzung.md' for match '[[Marktabgrenzung]]' ('Marktabgrenzung')
Dead link found: link 'C:\myvault\Abhängigkeit.md' for match '[[Abhängigkeit]]' ('Abhängigkeit')
Dead link found: link 'C:\myvault\kritischer Pfad.md' for match '[[kritischer Pfad]]' ('kritischer Pfad')
Dead link found: link 'C:\myvault\Tool.md' for match '[[Tool]]' ('Tool')
Dead link found: link 'C:\myvault\Evaluation.md' for match '[[Evaluation]]' ('Evaluation')
Dead link found: link 'C:\myvault\Produkt.md' for match '[[Produkt]]' ('Produkt')
Dead link found: link 'C:\myvault\Vermittler.md' for match '[[Vermittler]]' ('Vermittler')
Dead link found: link 'C:\myvault\Unternehmen.md' for match '[[Unternehmen]]' ('Unternehmen')
Dead link found: link 'C:\myvault\Karriere.md' for match '[[Karriere]]' ('Karriere')
Dead link found: link 'C:\myvault\Pfad.md' for match '[[Pfad]]' ('Pfad')
[...]
Dead link found: link 'C:\myvault\Gewinn.md' for match '[[Gewinn]]' ('Gewinn')
Dead link found: link 'C:\myvault\Mitarbeiter.md' for match '[[Mitarbeiter]]' ('Mitarbeiter')
Dead link found: link 'C:\myvault\Bekanntheitsgrad.md' for match '[[Bekanntheitsgrad]]' ('Bekanntheitsgrad')
Dead link found: link 'C:\myvault\Anforderung#Formulierung.md' for match '[[Anforderung#Formulierung|Anforderungsformulierung]]' ('Anforderung#Formulierung|Anforderungsformulierung')
Dead link found: link 'C:\myvault\Positionierung.md' for match '[[Positionierung]]' ('Positionierung')
Dead link found: link 'C:\myvault\Vergangenheit.md' for match '[[Vergangenheit]]' ('Vergangenheit')
Dead link found: link 'C:\myvault\Lesen.md' for match '[[Lesen]]' ('Lesen')
Dead link found: link 'C:\myvault\Person.md' for match '[[Person]]' ('Person')
Dead link found: link 'C:\myvault\Alternativen.md' for match '[[Alternativen]]' ('Alternativen')
Dead link found: link 'C:\myvault\Intuition.md' for match '[[Intuition]]' ('Intuition')
Cleaning up done, removed 584 dead links
===========================================
Finished plugin: Cleanup dead links
```

If you compare the two files you can see the links which got removed.
Tipp: Use a version control tool like Git to track changes.
![Example Report](https://dl.dropboxusercontent.com/s/wl4t4llh4jt8kys/2020-08-05_13-38-06.gif)

