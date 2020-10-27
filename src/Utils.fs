namespace Tuc.Domain

[<RequireQualifiedAccess>]
module Option =
    module Operators =
        let (=>) key value = (key, value)

[<RequireQualifiedAccess>]
module String =
    open System

    let toLower (value: string) =
        value.ToLower()

    let ucFirst (value: string) =
        match value |> Seq.toList with
        | [] -> ""
        | first :: rest -> (string first).ToUpper() :: (rest |> List.map string) |> String.concat ""

    let split (separator: string) (value: string) =
        value.Split(separator) |> Seq.toList

    let replaceAll (replace: string list) replacement (value: string) =
        replace
        |> List.fold (fun (value: string) toRemove ->
            value.Replace(toRemove, replacement)
        ) value

    let remove toRemove = replaceAll toRemove ""

    let append suffix string =
        sprintf "%s%s" string suffix

    let trimEnd (char: char) (string: string) =
        string.TrimEnd char

    let trimStart (char: char) (string: string) =
        string.TrimStart char

    let trim (char: char) (string: string) =
        string.Trim char

    let contains (subString: string) (string: string) =
        string.Contains(subString)

    let startsWith (prefix: string) (string: string) =
        string.StartsWith(prefix)

    let (|IsEmpty|_|): string -> _ = function
        | empty when empty |> String.IsNullOrEmpty -> Some ()
        | _ -> None

[<AutoOpen>]
module Regexp =
    open System.Text.RegularExpressions

    // http://www.fssnip.net/29/title/Regular-expression-active-pattern
    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ])
        else None

[<RequireQualifiedAccess>]
module List =
    /// see https://stackoverflow.com/questions/32363848/fastest-way-to-reduce-a-list-based-on-another-list-using-f
    let filterNotIn excluding list =
        let toExclude = set excluding
        list |> List.filter (toExclude.Contains >> not)

    let filterNotInBy f excluding list =
        let toExclude = set excluding
        list |> List.filter (f >> toExclude.Contains >> not)

    let filterInBy f including list =
        let toInclude = set including
        list |> List.filter (f >> toInclude.Contains)

    let formatLines linePrefix f = function
        | [] -> ""
        | lines ->
            let newLineWithPrefix = "\n" + linePrefix

            lines
            |> List.map f
            |> String.concat newLineWithPrefix
            |> (+) newLineWithPrefix

    let formatAvailableItems onEmpty onItems wantedItem definedItems =
        let normalizeItem =
            String.toLower

        let similarDefinedItem =
            definedItems
            |> List.tryFind (normalizeItem >> (=) (wantedItem |> normalizeItem))

        let availableItems =
            definedItems
            |> List.map (function
                | similarItem when (Some similarItem) = similarDefinedItem -> sprintf "%s  <--- maybe this one here?" similarItem
                | item -> item
            )

        match availableItems with
        | [] -> onEmpty
        | items -> items |> onItems

    /// It splits a list by a true/false result of the given function, when the first false occures, it will left all other items in false branch
    /// Example: [ 2; 4; 6; 7; 8; 9; 10 ] |> List.splitBy isEven results in ( [ 2; 4; 6 ], [ 7; 8; 9; 10 ] )
    let splitBy f list =
        let rec splitter trueBranch falseBranch f = function
            | [] -> trueBranch |> List.rev, falseBranch
            | i :: rest ->
                let trueBranch, falseBranch, rest =
                    if i |> f
                        then i :: trueBranch, falseBranch, rest
                        else trueBranch, falseBranch @ i :: rest, []

                rest |> splitter trueBranch falseBranch f

        list |> splitter [] [] f

[<AutoOpen>]
module Utils =
    let tee f a =
        f a
        a
