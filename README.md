# PunFinder
 For ancient Greek and Latin texts

### Features

- Returns a list of matches between ancient Greek and Latin texts
- Supports dipthongs, elision, accents, and breathing marks
- Matches are hilighted in original text along with matches for additional context

### Process

Once a Latin and Greek text(s) have been selected, the file paths are passed asynchronously into the model for processing. Both texts are then processed at the same time on different threads and the calling method waits for the completion of both methods. 

The Greek text string is first processed by stripping off any accent or breathing mark and returning the base letter to lower case. While iterating through all characters in the text, when a space or new line character is reached, the previous word captured is added to a list. A word is not added to the list if it has previously been added to the list or the length of the word is less than a number for which the program allows specification. The result of this method is a list of those unique Greek words which appear in a chosen Greek text.

The Latin text string is first processed by converting all letters to lower case and taking account of the rules of elision and pronunciation. Similar to the Greek processing, the processed Latin text is converted into words. Each Latin word in the list is processed to determine if part of the word should be elided, based on the starting letter of the next word. The final step is to condense all the Latin words together into one text string without spaces, while keeping a list of the starting position of each word in the text string.

Once the two processing methods reach completion, the model has the list of unique Greek words and a string of Latin words, including the starting position in the original text of each word.  The final processing method compares the Greek words to the Latin text string to find matches and returns a list of matches, which include the starting position of the match, ending position of the match, and the Greek word that matched.  First, each letter of every Greek word is converted to a list of possible Latin characters or pairs of characters. A dictionary exists that for each base Greek lower case letter there corresponds a list of Latin letters or pairs of letters. Similarly, the existence of a diphthong is determined by referencing the values of a Greek-Latin diphthong dictionary. Both dictionaries were developed by examining those grammars and references noted earlier. By iterating though every Greek letter in a Greek word the possible Latin letters can be looked up in the dictionary, and, if the current letter and next letter in the Greek word are in the diphthong dictionary, this will override the regular dictionary lookup function. Once a list of every possible Latin correspondence for every letter in a Greek word exists, all permutations of letters are returned in a list of strings. This list is the list of every possible Latin series of characters that could match a Greek word. Using a regular expression every match in the Latin text string is found based on the previous list. These results are returned as a list of Greek words, including the starting and ending position in the original Latin text of the match.

The unique list of Latin-to-Greek results is returned to the view and is displayed in a grid, which can be sorted alphabetically, wherein the left column contains the uncompressed Latin character matches and the right column the Greek word equivalent in Greek characters. Also, a flow document is created which contains the original text with the matches highlighted in yellow and the matching Greek words to the right for each line. The Greek words in brackets are words that correspond to the same Latin text location, thus assisting the program user in contextualizing and applying significance to the results.

###End