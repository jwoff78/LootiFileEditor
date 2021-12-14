# LootiFileEditor
Looti file Editor - Version official 1.0
<br><br>
Syntax:
```
Looti.Run(string[] argv);
```
<br>
Directly to Looti:

```
Looti.Editor(string[] argv);
```
Where argv[0] must be valid path to file.
<br>
<br>

## Arguments

Looti can be runned with few arguments which must be specified  into `string[] argv`.

### Arguments:

`--help` - help<br>
`--lock` - only to read mode<br>
`--ide` - Run Looti in IDE mode<br>

## Configuration file

Looti automatically creating `Looti.scf` with editor configuration. If configuration in `looti.scf` conflict with information in a variable
`string[] argv`, the information from `argv` is finally used. <br>

### Configuration:

`beep=yes/no` - beep on start<br>
`hello=yes/no` - show splash screen<br>
`click=yes/no` - make sound on click<br>
`wraplines=yes/no` - wrap lines<br>
`IDE=yes/no` - use Looti in IDE mode<br>
`cursor= ` - use " " (space) as cursor. If cursor char lenght is longer than one char, Looti uses " " (space) as cursor.

<br>Settings `IDE` and `click` are default `false`


