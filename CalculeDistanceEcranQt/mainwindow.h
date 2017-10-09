#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

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
    void on_radioButton1610_clicked();

    void on_radioButton169_clicked();

    void on_radioButton43_clicked();

    void on_size_valueChanged();

    void on_resolutionX_valueChanged();

private:
    Ui::MainWindow *ui;

    void computeDistance();
};

#endif // MAINWINDOW_H
