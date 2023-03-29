# General best practices

### Naming

- Use good naming for functions and classes
  - Classes are named as a noun - what is it
  - Methods are named as a verb - what does it do
  - Use as concrete naming as possible
  - Name length should be proportional to the item's scope

### Nesting & spacing

- Split blocks of logic with a blank line
- Return as early as possible to avoid nesting

### Other

- Explicit is better than implicit in almost any case
- If the exact piece code is repeated at least twice, consider extracting repeating logic
- Comments are not good unless they explain some hard or not explicit logic 
- In general, code should be written in such way that it shouldn't require explicit comments.

# Project code style

### File naming & content rules

- Files are named in PascalCase.
- Files should contain 1 item: class/struct/record/interface/enum, and should be named after that item.

### Item naming

- Constants: PascalCase
- Variables: camelCase
- Private fields: _camelCase
- Types: PascalCase
- Interfaces: PascalCase
- Methods: PascalCase

### Code formatting & spacing

- Do not use tabs
- Use 4-spaces indentation
- Place spaces after commas in arrays or objects
- Separate class member type by 1 blank line from each other

### Typical class organization:

1. Private readonly fields
2. Private non-readonly fields
3. Private contants
4. Public contants
5. Public properties
6. Lifecycle members:
   1. Constructors
   2. Dispose method
   3. Deconstructors
7. Public methods
8. Private methods


### General best practices

- Put member initialization as close to its usage as possible
- Use .NET 6 file-scoped namespace to reduce initial nesting by 1 level
- If 'throw' or 'if' constructions don't require additional actions, put it on the next line without braces
- If nested field is accessed multiple times, extract is as a local variable
- Place constructor arguments each on the next line
- Place chained methods each on the next line
- When a delegate contains multiple lines, align as left as possible (press enter after method opening brace usually helps)
