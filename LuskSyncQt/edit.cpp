#include "edit.h"
#include "ui_edit.h"

Edit::Edit(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Edit)
{
    ui->setupUi(this);
}

Edit::~Edit()
{
    delete ui;
}

//ftpPath accessors
void Edit::setFtpPath(const QString& text)
{
    ui->ftpPath->setText(text);
}

QString Edit::getFtpPath() const
{
    return ui->ftpPath->toPlainText();
}

//localPath accessors
void Edit::setLocalPath(const QString& text)
{
    ui->localPath->setText(text);
}

QString Edit::getLocalPath() const
{
    return ui->localPath->toPlainText();
}

//name accessors
void Edit::setName(const QString& text)
{
    ui->name->setText(text);
}

QString Edit::getName() const
{
    return ui->name->toPlainText();
}
