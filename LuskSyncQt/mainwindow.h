#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QStandardItemModel>
#include <QDir>

//use local qftp for newer and qt one for older
#include <QtGlobal>
#if QT_VERSION >= 0x050000
    #include "QtFtp/qftp.h"
#else
    #include <qftp>
#endif



namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

private slots:
    void on_PushButton_Add_clicked();

    void on_pushButton_Delete_clicked();

    void on_pushButton_Edit_clicked();

    void on_pushButton_Launch_clicked();

private:
    Ui::MainWindow *ui;

    void initTableView();
    QList<QStandardItem *> prepareRow(const QString &p1, const QString &p2, const QString &p3, const QString &p4);

    void searchSyncFiles();

    QStandardItemModel *modelMain;
    QStandardItemModel *modelDetail;
    QFtp* ftp;

    QFileInfoList localFiles;
    QList<QUrlInfo> remoteFiles;

 private slots:
    //void commandStarted(int id);
    void addToList(const QUrlInfo &urlInfo);
    //void commandFinished(int id, bool error);
    void done(bool error);
};

#endif // MAINWINDOW_H
