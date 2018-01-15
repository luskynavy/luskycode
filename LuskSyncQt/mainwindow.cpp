#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "edit.h"

#include <QDebug>
#include <QUrl>

#include <fstream>
#include <iostream>
#include <string>
#include <cstdio>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    initTableView();
    searchSyncFiles();
}

void MainWindow::initTableView()
{
    modelMain = new QStandardItemModel(0, 4, this); //2 Rows and 3 Columns
    modelMain->setHorizontalHeaderItem(0, new QStandardItem(QString("Name")));
    modelMain->setHorizontalHeaderItem(1, new QStandardItem(QString("Source")));
    modelMain->setHorizontalHeaderItem(2, new QStandardItem(QString("Dest")));
    modelMain->setHorizontalHeaderItem(3, new QStandardItem(QString("Last")));

    ui->tableView->setModel(modelMain);

    modelDetail = new QStandardItemModel(0, 4, this); //2 Rows and 4 Columns
    modelDetail->setHorizontalHeaderItem(0, new QStandardItem(QString("Name")));
    modelDetail->setHorizontalHeaderItem(1, new QStandardItem(QString("Size")));
    modelDetail->setHorizontalHeaderItem(2, new QStandardItem(QString("Date")));
    modelDetail->setHorizontalHeaderItem(3, new QStandardItem(QString("Time")));
}

MainWindow::~MainWindow()
{
    delete ui;
}

QList<QStandardItem *> MainWindow::prepareRow(const QString &p1, const QString &p2, const QString &p3, const QString &p4)
{
    QList<QStandardItem *> rowItems;
    rowItems << new QStandardItem(p1);
    rowItems << new QStandardItem(p2);
    rowItems << new QStandardItem(p3);
    rowItems << new QStandardItem(p4);

    return rowItems;
}

void MainWindow::searchSyncFiles()
{
    //search all *.sncin current dir
    QDir dir("");
    QStringList filters;
    filters << "*.snc";
    QFileInfoList syncFiles = dir.entryInfoList(filters, QDir::Files);

    for(auto f : syncFiles)
    {
        qDebug() << f.fileName() << " " << f.size() << (f.isDir() ? "DIR" : "FIL")
                 << " " << f.lastModified().date().toString("yyyy-MM-dd")
                 << " " << f.lastModified().time().toString();

        //add content to list
        std::string /*name, */ftpPath, localPath;
        std::ifstream fileSync(f.fileName().toStdString());
        if (fileSync)
        {
            //getline(fileSync, name);
            getline(fileSync, ftpPath);
            getline(fileSync, localPath);

            //name is filename minus ".snc"
            QList<QStandardItem *> row = prepareRow(f.fileName().left(f.fileName().length() - 4)/*QString::fromStdString(name)*/,
                                                    QString::fromStdString(ftpPath), QString::fromStdString(localPath), "");
            //add line to view
            modelMain->appendRow(row);

            fileSync.close();
        }
    }
}

void MainWindow::saveSyncFile(const QString &name, const QString &ftpPath, const QString &localPath)
{
    std::ofstream fileSync(name.toStdString() + ".snc");
    if (fileSync)
    {
        fileSync << ftpPath.toStdString() << '\n';
        fileSync << localPath.toStdString() << '\n';

        fileSync.close();
    }
}

void MainWindow::on_PushButton_Add_clicked()
{

    /*QList<QStandardItem *> row = prepareRow("truc" + QString::number(rand() % 100), "E:\\Users\\yvan.kalafatov\\Documents\\My Games\\SteamWorld Dig\\", "/syncback/SteamWorld Dig", "2017-11-10 15:50:10");
    modelMain->appendRow(row);*/

    Edit e;
    e.setName("test");

    int result = e.exec();
    qDebug() << "result " << result;

    //if ok selected update tableview and save
    if (result == QDialog::Accepted)
    {
        //if name for file is not empty
        if (e.getName().length())
        {
            QList<QStandardItem *> row = prepareRow(e.getName(), e.getFtpPath(), e.getLocalPath(), "");
            modelMain->appendRow(row);

            saveSyncFile(e.getName(), e.getFtpPath(), e.getLocalPath());
        }

        ui->tableView->setModel(modelMain);
    }
}

void MainWindow::on_pushButton_Delete_clicked()
{
    QItemSelectionModel *selected = ui->tableView->selectionModel();
    if (selected->hasSelection())
    {
        int rowId = selected->selectedRows().at(0).row();

        //convert qstring to char*
        QString fileName = modelMain->index(rowId, 0).data().toString() + ".snc";
        QByteArray nameBA = fileName.toLatin1();
        const char *nameCStr = nameBA.data();

        qDebug() << fileName << nameCStr;

        //delete file
        std::remove(nameCStr);

        //update tableview
        modelMain->removeRow(rowId);

        ui->tableView->setModel(modelMain);
    }
}

void MainWindow::on_pushButton_Edit_clicked()
{
    QItemSelectionModel *selected = ui->tableView->selectionModel();
    if (selected->hasSelection())
    {
        int rowId = selected->selectedRows().at(0).row();

        qDebug() << "edit " << rowId;

        //show edit dialog box with values
        Edit e;
        QString nameBeforeEdit;

        nameBeforeEdit = modelMain->index(rowId, 0).data().toString();
        e.setName(nameBeforeEdit);
        e.setFtpPath(modelMain->index(rowId, 1).data().toString());
        e.setLocalPath(modelMain->index(rowId, 2).data().toString());

        int result = e.exec();
        qDebug() << "result " << result;

        //if ok selected update tableview and save
        if (result == QDialog::Accepted)
        {
            qDebug() << "getFtpPath " << e.getName();
            //update tableview
            modelMain->item(rowId, 0)->setData(e.getName(), Qt::DisplayRole);
            modelMain->item(rowId, 1)->setData(e.getFtpPath(), Qt::DisplayRole);
            modelMain->item(rowId, 2)->setData(e.getLocalPath(), Qt::DisplayRole);

            //write the file with old name
            saveSyncFile(nameBeforeEdit, e.getFtpPath(), e.getLocalPath());

            //rename old file to new name if changed
            if (nameBeforeEdit != e.getName())
            {
                QByteArray nameBeforeEditBA = nameBeforeEdit.toLatin1() + ".snc";
                const char *nameBeforeEditCStr = nameBeforeEditBA.data();
                QByteArray newBA = e.getName().toLatin1() + ".snc";
                const char *newCStr = newBA.data();

                std::rename(nameBeforeEditCStr, newCStr);
            }
        }
    }

    /*//only files
    QDir dir("E:\\Users\\yvan.kalafatov\\Documents\\My Games\\SteamWorld Dig\\");
    localFiles = dir.entryInfoList(QDir::Files);

    modelDetail->removeRows(0, modelDetail->rowCount());
    ui->tableView->setModel(modelDetail);

    for(auto f : localFiles)
    {
        qDebug() << f.fileName() << " " << f.size() << (f.isDir() ? "DIR" : "FIL")
                 << " " << f.lastModified().date().toString("yyyy-MM-dd")
                 << " " << f.lastModified().time().toString();

        QList<QStandardItem *> row = prepareRow(f.fileName(), QString::number(f.size()), f.lastModified().date().toString("yyyy-MM-dd"), f.lastModified().time().toString());
        modelDetail->appendRow(row);
    }
    qDebug() << "";*/
}

void MainWindow::on_pushButton_Launch_clicked()
{
    QItemSelectionModel *selected = ui->tableView->selectionModel();
    if (selected->hasSelection())
    {
        int rowId = selected->selectedRows().at(0).row();

        qDebug() << "launch " << rowId;        
    }

    ftp = new QFtp(this);
    //connect(ftp, SIGNAL(commandStarted(int)), this, SLOT(commandStarted(int)));
    connect(ftp, SIGNAL(listInfo(QUrlInfo)), this, SLOT(addToList(QUrlInfo)));
    //connect(ftp, SIGNAL(commandFinished(int, bool)), this, SLOT(commandFinished(int, bool)));
    connect(ftp, SIGNAL(done(bool)), this, SLOT(done(bool)));

    remoteFiles.clear();

    QUrl url("ftp://test:truc2@localhost:21/");
    if (!url.isValid() || url.scheme().toLower() != QLatin1String("ftp")) {
        ftp->connectToHost(url.host(), 21);
        ftp->login();
    } else {
        ftp->connectToHost(url.host(), url.port(21));

        if (!url.userName().isEmpty())
            ftp->login(QUrl::fromPercentEncoding(url.userName().toLatin1()), url.password());
        else
            ftp->login();
        if (!url.path().isEmpty())
            ftp->cd(url.path());
        ftp->list();
    }    
}

//ftp signal for new entry for listInfo
void MainWindow::addToList(const QUrlInfo &urlInfo)
{
    //only files
    if (!urlInfo.isDir())
    {
        qDebug() << urlInfo.name() << " " << urlInfo.size() << (urlInfo.isDir() ? "DIR" : "FIL")
                 << " " << urlInfo.lastModified().date().toString("yyyy-MM-dd")
                 << " " << urlInfo.lastModified().time().toString();

        remoteFiles.append(urlInfo);
    }
}

/*void MainWindow::commandStarted(int id)
{
    qDebug() << id << ftp->currentId();
}

void MainWindow::commandFinished(int id, bool error)
{
    qDebug() << id << error << ftp->currentId();
}*/

//ftp signal when listInfo is done
void MainWindow::done(bool error)
{
    qDebug() << " " << error;
    ui->tableView->setModel(modelDetail);
    modelDetail->removeRows(0, modelDetail->rowCount());

    for(auto f : remoteFiles)
    {
        QList<QStandardItem *> row = prepareRow(f.name(), QString::number(f.size()),
                                                f.lastModified().date().toString("yyyy-MM-dd"),
                                                f.lastModified().time().toString());
        modelDetail->appendRow(row);
    }
}
