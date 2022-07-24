# Accounts

Manage a hierarchy of monetary accounts. You can define one-off transactions and recurring transactions.

Currently this is a console app only. However the code has been written to support single-user and multi-user
scenarios. Future iterations may include an HTTP API and a web/mobile front end in .NET MAUI.

## Installing the console app from pre-built binaries
Download the latest ZIP under Releases and unzip it to a temporary directory.

### Linux and macOS
From within the temporary directory:

```
./install.sh RID
```

where `RID` represents the runtime identifier for your OS and architecture. If you do not specify a RID, the default is
`osx.12-arm64` (macOS Monterey on ARM64).

Currently the following RIDs are supported:

* osx.12-x64
* osx.12-arm64
* linux-x64
* linux-arm
* linux-arm64

The script will create a symlink called `accounts` in `~/bin`, so please ensure that that directory exists and is in your path.

### Windows 10 and 11
Move the directory for your architecture (either `win10-x64` or `win10-arm64`) to a
convenient permanent location then create a shortcut to `Morphologue.Accounts.Presentation.ConsoleApp.exe` therein.

## Building and running the console app from source
You must have the .NET >=6 SDK installed.

Then in the `src/Morphologue.Accounts.Presentation.ConsoleApp` directory:

```
$ dotnet run -- --help
```

To run the tests, change to the `src/Morphologue.Accounts.UnitTests` directory and execute:

```
$ dotnet test
```

## Basic usage
Create an account under the root account:

```
$ accounts add account "Primary"
```

Create a sub-account:

```
$ accounts add account --parent "Primary" "Secondary"
```

List accounts:

```
$ accounts list accounts

[Root]
  Primary
    Secondary
```

Add a one-off transaction with today's date and no description:

```
$ accounts add transaction --account "Secondary" 16.35
```

Add a transaction which recurs quarterly from the given date and has a negative amount:

```
$ accounts add transaction --account "Primary" --description "Demo" --date 2022-01-21 --recurring 3m '$-10.69'
```

List transactions:

```
$ list transactions --since 2022-01-01

ID     Date       Amount      Balance     Description                              Account
    2* 2022-01-21 $    -10.69 $    -10.69 Demo                                     Primary
    2* 2022-04-21 $    -10.69 $    -21.38 Demo                                     Primary
    2* 2022-07-21 $    -10.69 $    -32.07 Demo                                     Primary
    1  2022-07-24 $     16.35 $    -15.72                                          Secondary
```

Change a transaction:
```
$ accounts change transaction --recurring 84d 2

$ accounts list transactions --since 2022-01-01

ID     Date       Amount      Balance     Description                              Account
    2* 2022-01-21 $    -10.69 $    -10.69 Demo                                     Primary
    2* 2022-04-15 $    -10.69 $    -21.38 Demo                                     Primary
    2* 2022-07-08 $    -10.69 $    -32.07 Demo                                     Primary
    1  2022-07-24 $     16.35 $    -15.72                                          Secondary
```

## Advanced usage

The `--help` output shows the options which are available for each command and subcommand. For example:

```
$ accounts delete --help

Usage: accounts delete [command] [options]

Options:
  -?|-h|--help  Show help information.

Commands:
  account       
  transaction   

Run 'delete [command] -?|-h|--help' for more information about a command.
```

Note also that "account(s)" and "transaction(s)" may be abbreviated as follows:

| In `--help` output | Abbreviation |
|--------------------|--------------|
| account            | acct         |
| accounts           | accts        |
| transaction        | tran         |
| transaction        | trans        |

So the following command (also shown above) —

```
$ accounts add transaction --account "Primary" --description "Demo" --date 2022-01-21 --recurring 3m '$-10.69'
```

— may be abbreviated as follows:

```
$ accounts add tran -a Primary -d Demo -y 2022-01-21 -r 3m '$-10.69'
```

Some additional functionality to note:
* An account may be closed using the `change transaction` command with the `--close` option. One-off transactions and
recurring transaction instances after the closure date will not be shown. Also the account itself may be hidden
depending on the `--date` option of the `show accounts` command.
* A child account is automatically closed when one of its ancestors is closed.
* Running totals include the children of the account which is being listed.

## Licence
[GNU GPL v3](LICENSE)
