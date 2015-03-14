#include <stdio.h>
#include <iostream>
#include <fstream>
#include <string>
//#include <string.h>


using namespace std;

	char words[10];
	char iniwordstemp[50];
	char iniwords[100000];
	string temp[100];
	string ID[100];
	int w=0;
	int n=0,m=0;
	int flag=0;
	char key[8][8]={"if","else","for","while","do","int","write","read"};
	ifstream fin("d:/1.txt");
	ofstream fout("d:/2.txt");
int issame(string a,char b[8])
	{
		int c=strlen(b);
		int	d=(int)a.size();
		if (d==c)
		{
			for (int i=0;i<d;i++)
			{
				if (a[i]==b[i])
				{
					continue;
				}
				else
				{
					return 0;
				}
			}
			return 1;
		}
 		else
 		{
 			return 0;
 		}
	}
void scan(char t[100000])
{
	char ch,ch1;
	int p=0;
	int m=0;
	ch=t[p];
	p++;
	while(p<strlen(t)+1)
	{

		while(ch==' ')
		{
			ch=t[p];
			p++;
		}
		for (int i=0;i<10;i++)
		{
			words[i]=NULL;
		}

		if ((ch>='a'&&ch<='z')||(ch>='A'&&ch<='Z'))
		{

			m=0;
			while ((ch>='0'&&ch<='9')||(ch>='a'&&ch<='z')||(ch>='A'&&ch<='Z'))
			{
				words[m]=ch;
				m++;
				ch=t[p];
				p++;
			}
			flag=0;
			temp[w]=words;
			for (int n=0;n<8;n++)
			{
 				string t=(string)temp[1];
				if (issame(temp[w],key[n]))
				{
					flag=1;
					break;
				}
			}
			
			//cout<<flag<<endl;
			if (flag==1)
			{
				flag=0;
				fout<<temp[w]<<"	 "<<temp[w]<<"\n";
			}
			else
			{
				fout<<"ID	 "<<temp[w]<<"\n";
			}
			w++;
		}
		else if (ch>='0'&&ch<='9')
		{
			m=0;
			while(ch>='0'&&ch<='9')
			{
				words[m]=ch;
				m++;
				ch=t[p];
				p++;
			}
			temp[w]=words;
			fout<<"NUM	 "<<temp[w]<<"\n";
			w++;
		}
		else if (ch=='>'||ch=='<'||ch=='!'||ch=='=')
		{
				m=0;
				words[m]=ch;
				m++;
				ch=t[p];
				p++;
				if (ch=='=')
				{
					words[m]=ch;
					m++;
					temp[w]=words;
					fout<<temp[w]<<"	 "<<temp[w]<<"\n";
					w++;
					ch=t[p];
					p++;
				}
				else
				{
					temp[w]=words;
					fout<<temp[w]<<"	"<<temp[w]<<"\n";
					w++;
				}
		}
		else if (ch=='+'||ch=='-'||ch=='*'||ch=='('||ch==')'||ch==';'||ch=='{'||ch=='}')
		{
			m=0;
			words[m]=ch;
			m++;
			ch=t[p];
			p++;
			temp[w]=words;
			fout<<temp[w]<<"	 "<<temp[w]<<"\n";
			w++;
		}
		else if (ch=='/')
		{
			m=0;
			words[m]=ch;
			m++;
			ch=t[p];
			p++;
			if (ch=='*')
			{
				ch=t[p];
				ch1=t[p+1];
				p++;
				while(1)
				{
					if ((ch=='*'&&ch1=='/'))
					{
						break;
					}
					else if (ch1=='\0')
					{
						cout<<"/**/ error \n";
						break;
					}
					else
					{
						ch=t[p];
						ch1=t[p+1];
						p++;
					}
				}
				ch=t[p];
				p++;
				ch=t[p];
				p++;

			}
			else
			{
				temp[w]=words;
				fout<<temp[w]<<"	 "<<temp[w]<<"\n";
				w++;
			}
		}
		else
		{
			m=0;
			words[m]=ch;
			ch=t[p];
			p++;
			cout<<"error "<<words[m]<<"\n";
		}
	}
	
}


int main()
{


	while(fin.getline(iniwordstemp,50))
	{
		
		for (int i=0;i<strlen(iniwordstemp);i++)
		{
			iniwords[n]=iniwordstemp[i];
			n++;
		}
		iniwords[n]=' ';
		n++;
	}
	scan(iniwords);
	return 0;
	
}