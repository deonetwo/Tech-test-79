CREATE TABLE Accounts (
    Name varchar(100) NOT NULL,
    AccountId int NOT NULL,
    CONSTRAINT Accounts_PK PRIMARY KEY (AccountId)
);

CREATE TABLE AccountTransactions (
    TransactionId int IDENTITY(0,1) NOT NULL,
    TransactionDate datetime NOT NULL,
    Description varchar(100) NULL,
    DebitCreditStatus char(1) NOT NULL,
    Amount money NULL,
    AccountId int NOT NULL,
    CONSTRAINT AccountTransactions_PK PRIMARY KEY (TransactionId),
    CONSTRAINT AccountTransactions_FK FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId)
);